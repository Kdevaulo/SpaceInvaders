using System;
using System.Collections.Generic;
using System.Linq;

using Kdevaulo.SpaceInvaders.BulletBehaviour;
using Kdevaulo.SpaceInvaders.LevelSystem;
using Kdevaulo.SpaceInvaders.ScoreBehaviour;

using UniRx;
using UniRx.Triggers;

using UnityEngine;

using Zenject;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Kdevaulo.SpaceInvaders.EnemiesBehaviour
{
    public sealed class EnemiesController : ITickable, IPauseHandler, IDisposable, IResourceHandler
    {
        private const float MoveStepDivider = 10;

        [Inject]
        private ScreenUtilities _screenUtilities;

        [Inject]
        private LevelingService _levelingService;

        [Inject]
        private ScoreService _scoreService;

        [Inject]
        private BulletService _bulletService;

        private EnemyModel[,] _enemiesArray;
        private List<EnemyModel> _enemies = new List<EnemyModel>();

        private CompositeDisposable _disposable = new CompositeDisposable();

        private bool _isPaused;
        private bool _isInitialized;
        private bool _isLeftDirection;

        private int _maxEnemies;

        private float _verticalStep;
        private float _shootDelay;
        private float _moveTimeCounter;
        private float _shootTimeCounter;
        private float _currentMoveDelay;

        private Vector2 _moveDelayBounds;
        private Vector2 _bulletDirection;

        private AnimationCurve _speedFunction;

        private Rect _screenRect;

        public void Initialize(List<EnemyModel> enemies, EnemyModel[,] enemiesArray, Vector2 moveDelayBounds,
            AnimationCurve speedFunction, float verticalStep, float shootDelay, Vector2 bulletDirection)
        {
            _enemies = enemies;
            _shootDelay = shootDelay;
            _enemiesArray = enemiesArray;
            _verticalStep = verticalStep;
            _speedFunction = speedFunction;
            _moveDelayBounds = moveDelayBounds;
            _bulletDirection = bulletDirection;
            _currentMoveDelay = moveDelayBounds.x;

            _shootTimeCounter = _shootDelay;
            _moveTimeCounter = _currentMoveDelay;

            _maxEnemies = _enemies.Count;
            _screenRect = _screenUtilities.GetScreenRectInUnits();

            foreach (var enemy in _enemies)
            {
                var view = enemy.View;

                view.Collider.OnTriggerEnter2DAsObservable()
                    .Where(x => x.CompareTag(enemy.VulnerableProjectileTag))
                    .Subscribe(_ => HandleKilledEvent(enemy))
                    .AddTo(_disposable);
            }

            _isLeftDirection = false;
            _isInitialized = true;
        }

        void IResourceHandler.Release()
        {
            foreach (var enemy in _enemies)
            {
                Object.Destroy(enemy.View.gameObject);
            }

            _enemies.Clear();

            _disposable.Clear();
            _isInitialized = false;
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
            _isInitialized = false;
        }

        void IPauseHandler.HandlePause()
        {
            _isPaused = true;
        }

        void IPauseHandler.HandleResume()
        {
            _isPaused = false;
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

        private void HandleKilledEvent(EnemyModel model)
        {
            Object.Destroy(model.View.gameObject);
            _enemies.Remove(model);

            _enemiesArray[model.Index.x, model.Index.y] = null;

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

            DoWithEach(enemy => enemy.Position += offset / MoveStepDivider);
        }

        private void HandleScreenCollisions()
        {
            bool switchDirection = _enemies.Any(enemy => enemy.IsOutOfBoundsHorizontal(_screenRect));

            if (switchDirection)
            {
                MoveVertical();
                _isLeftDirection = !_isLeftDirection;
            }

            bool outOfBounds = _enemies.Any(enemy => enemy.IsOutOfBoundsVertical(_screenRect));

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

            int enemyIndex = Random.Range(0, shooters.Count);
            var startPosition = shooters[enemyIndex].Position;
            string shooterTag = "Enemy"; //todo: get shooter tag

            _bulletService.AddBullet(_bulletDirection, startPosition, shooterTag, shooterTag);
        }
    }
}