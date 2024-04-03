using System;
using System.Collections.Generic;
using System.Linq;

using Kdevaulo.SpaceInvaders.LevelSystem;
using Kdevaulo.SpaceInvaders.ScoreBehaviour;

using UniRx;
using UniRx.Triggers;

using UnityEngine;

using Zenject;

using Object = UnityEngine.Object;

namespace Kdevaulo.SpaceInvaders.EnemiesBehaviour
{
    public sealed class EnemiesController : ITickable, IPauseHandler, IDisposable, IResourceHandler
    {
        [Inject]
        private ScreenUtilities _screenUtilities;

        [Inject]
        private LevelingService _levelingService;

        [Inject]
        private ScoreService _scoreService;

        private List<EnemyModel> _enemies = new List<EnemyModel>();

        private CompositeDisposable _disposable = new CompositeDisposable();

        private bool _isLeftDirection;
        private bool _isPaused;
        private bool _isInitialized;

        private float _currentSpeed;
        private float _speedStep;
        private float _verticalStep;

        private Rect _screenRect;

        public void Initialize(List<EnemyModel> enemies, float startSpeed, float speedStep, float verticalStep)
        {
            _enemies = enemies;
            _currentSpeed = startSpeed;
            _speedStep = speedStep;
            _verticalStep = verticalStep;

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

            MoveHorizontal();
            HandleScreenCollisions();
        }

        private void HandleKilledEvent(EnemyModel model)
        {
            Object.Destroy(model.View.gameObject);
            _enemies.Remove(model);
            _currentSpeed += _speedStep;

            _scoreService.AddScore(model.RewardPoints);

            if (_enemies.Count == 0)
            {
                _levelingService.StartNewStage();
            }
        }

        private void MoveHorizontal()
        {
            var offset = _isLeftDirection ? Vector2.left : Vector2.right;

            DoWithEach(enemy => enemy.Position += offset * _currentSpeed);
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
                _scoreService.ClearScore();
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
    }
}