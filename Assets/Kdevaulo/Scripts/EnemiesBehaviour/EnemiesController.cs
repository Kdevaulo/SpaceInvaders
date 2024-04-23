using System;
using System.Collections.Generic;
using System.Linq;

using Kdevaulo.SpaceInvaders.DropBehaviour;
using Kdevaulo.SpaceInvaders.LevelngSystem;
using Kdevaulo.SpaceInvaders.ProjectileBehaviour;
using Kdevaulo.SpaceInvaders.ScoreSystem;
using Kdevaulo.SpaceInvaders.ScreenSystem;

using UniRx;
using UniRx.Triggers;

using UnityEngine;
using UnityEngine.Assertions;

using Zenject;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Kdevaulo.SpaceInvaders.EnemiesBehaviour
{
    public sealed class EnemiesController : IInitializable, ITickable, IPauseHandler, IDisposable, IResourceHandler
    {
        bool IPauseHandler.IsPaused
        {
            set => _isPaused = value;
        }

        [Inject] private DropService _dropService;
        [Inject] private ScoreService _scoreService;
        [Inject] private ScreenService _screenService;
        [Inject] private LevelingService _levelingService;
        [Inject] private ProjectileService _projectileService;

        [Inject] private LevelingModel _levelingModel;

        [Inject] private EnemiesFactory _factory;
        [Inject] private PositionsProvider _positionsProvider;

        private List<EnemyModel> _enemies;
        private EnemyModel[,] _enemiesArray;

        private CompositeDisposable _eventsDisposable = new CompositeDisposable();
        private CompositeDisposable _collisionDisposable = new CompositeDisposable();

        private LevelSettingsData _levelSettings;

        private bool _isPaused;
        private bool _isInitialized;
        private bool _isLeftDirection;

        private int _maxEnemies;

        private float _shootDelay;
        private float _verticalStep;
        private float _moveTimeCounter;
        private float _moveStepDivider;
        private float _shootTimeCounter;
        private float _currentMoveDelay;

        private Vector2 _moveDelayBounds;
        private Vector2 _bulletDirection;

        private AnimationCurve _speedFunction;

        private Rect _screenRect;

        void IInitializable.Initialize()
        {
            _levelingModel.LevelSettings.Where(x => x != null).Subscribe(_ => Prepare()).AddTo(_eventsDisposable);
        }

        void ITickable.Tick()
        {
            if (_isPaused || !_isInitialized)
            {
                return;
            }

            _moveTimeCounter -= Time.deltaTime;
            _shootTimeCounter -= Time.deltaTime;

            if (_moveTimeCounter <= 0)
            {
                MoveHorizontal();
                HandleScreenCollisions();
                _moveTimeCounter = _currentMoveDelay;
            }

            if (_shootTimeCounter <= 0)
            {
                Shoot();
                _shootTimeCounter = _shootDelay;
            }
        }

        void IResourceHandler.Release()
        {
            foreach (var enemy in _enemies)
            {
                Object.Destroy(enemy.View.gameObject);
            }

            _enemies.Clear();

            _collisionDisposable.Clear();
            _isInitialized = false;
        }

        void IDisposable.Dispose()
        {
            _eventsDisposable.Dispose();
            _collisionDisposable.Dispose();
            _isInitialized = false;
        }

        private void Prepare()
        {
            _levelSettings = _levelingModel.LevelSettings.Value;
            InitializeFields(_levelSettings);

            CreateEnemies(_levelSettings.EnemiesSettings, _levelSettings.EnemiesColumnsCount);
            PlaceEnemies(_enemies);

            SubscribeEvents();

            _isLeftDirection = false;
            _isInitialized = true;
        }

        private void InitializeFields(LevelSettingsData levelSettings)
        {
            _screenRect = _screenService.GetScreenRectInUnits();

            _shootDelay = levelSettings.EnemyShootDelay;
            _verticalStep = levelSettings.EnemyVerticalStep;
            _moveStepDivider = levelSettings.EnemyMoveStepDivider;
            _bulletDirection = levelSettings.EnemiesBulletDirection;
            _moveDelayBounds = levelSettings.EnemiesMoveDelayBounds;
            _speedFunction = levelSettings.EnemyMovementSpeedPattern;
            _currentMoveDelay = levelSettings.EnemiesMoveDelayBounds.x;

            _shootTimeCounter = _shootDelay;
            _moveTimeCounter = _currentMoveDelay;
        }

        private void CreateEnemies(EnemySettings[] data, int columns)
        {
            _enemies = _factory.Create(data);
            _maxEnemies = _enemies.Count;

            _enemiesArray =
                ArrayUtilities<EnemyModel>.PackItems(_enemies, columns, (item, index) => item.Index = index);
        }

        private void PlaceEnemies(List<EnemyModel> enemies)
        {
            var positions = _positionsProvider.GetPositions();

            int enemiesCount = enemies.Count;

            Assert.IsTrue(enemiesCount <= positions.Length, "Positions count is less than enemies count");

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Position = positions[i];
            }
        }

        private void SubscribeEvents()
        {
            foreach (var enemy in _enemies)
            {
                var view = enemy.View;

                view.Collider.OnTriggerEnter2DAsObservable()
                    .Where(x => x.IfAnyTag(enemy.VulnerableObjectsTags))
                    .Subscribe(_ => HandleKilledEvent(enemy))
                    .AddTo(_collisionDisposable);
            }
        }

        private void HandleKilledEvent(EnemyModel model)
        {
            Object.Destroy(model.View.gameObject);
            _enemies.Remove(model);

            _enemiesArray[model.Index.x, model.Index.y] = null;

            if (Random.value > 0.3f)
            {
                _dropService.Add(model.DropType, model.Position);
            }

            if (_enemies.Count > 0)
            {
                float t = _speedFunction.Evaluate(1f - _enemies.Count / (float) _maxEnemies);
                _currentMoveDelay = Mathf.Lerp(_moveDelayBounds.x, _moveDelayBounds.y, t);
            }

            _scoreService.Add(model.RewardPoints);

            if (_enemies.Count == 0)
            {
                _levelingService.StartNewStage();
            }
        }

        private void MoveHorizontal()
        {
            var offset = _isLeftDirection ? Vector2.left : Vector2.right;

            DoWithEach(enemy => enemy.Position += offset / _moveStepDivider);
        }

        private void HandleScreenCollisions()
        {
            bool switchDirection = _enemies.Any(enemy =>
                Utilities.IsOutOfBoundsHorizontal(enemy.Position.x, enemy.HalfHorizontalSize, _screenRect));

            if (switchDirection)
            {
                MoveVertical();
                _isLeftDirection = !_isLeftDirection;
            }

            bool outOfBounds = _enemies.Any(enemy =>
                Utilities.IsOutOfBoundsVertical(enemy.Position.y, enemy.HalfVerticalSize, _screenRect));

            if (outOfBounds)
            {
                _levelingService.Restart();
                _scoreService.Clear();
            }
        }

        private void MoveVertical()
        {
            DoWithEach(enemy => enemy.Position += new Vector2(0, _verticalStep));
        }

        private void DoWithEach(Action<EnemyModel> action)
        {
            foreach (var enemy in _enemies)
            {
                action.Invoke(enemy);
            }
        }

        private void Shoot()
        {
            int rows = _enemiesArray.GetLength(0);
            int columns = _enemiesArray.GetLength(1);

            var shooters = new List<EnemyModel>();

            for (int column = 0; column < columns; column++)
            {
                for (int row = 0; row < rows; row++)
                {
                    var enemy = _enemiesArray[row, column];

                    if (enemy != null)
                    {
                        shooters.Add(enemy);
                        break;
                    }
                }
            }

            if (shooters.Count > 0)
            {
                int enemyIndex = Random.Range(0, shooters.Count);
                var startPosition = shooters[enemyIndex].Position;

                _projectileService.AddBullet(_bulletDirection, startPosition, _levelSettings.EnemyBulletIgnoreTags,
                    _levelSettings.EnemyProjectileTag);
            }
        }
    }
}