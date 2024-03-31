using System;
using System.Collections.Generic;
using System.Linq;

using Kdevaulo.SpaceInvaders.LevelSystem;

using UniRx;
using UniRx.Triggers;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

using Object = UnityEngine.Object;

namespace Kdevaulo.SpaceInvaders.EnemiesBehaviour
{
    public sealed class EnemiesController : ITickable, IPauseHandler, IDisposable
    {
        [Inject]
        private Rect _canvasRect;

        [Inject]
        private CanvasScaler _canvasScaler;

        [Inject]
        private LevelingService _levelingService;

        private List<EnemyModel> _enemies;

        private CompositeDisposable _disposable = new CompositeDisposable();

        private bool _isLeftDirection;
        private bool _isPaused;
        private bool _initialized;

        private float _currentSpeed;
        private float _speedStep;
        private float _verticalStep;

        public void Initialize(List<EnemyModel> enemies, float startSpeed, float speedStep, float verticalStep)
        {
            _enemies = enemies;
            _currentSpeed = startSpeed;
            _speedStep = speedStep;
            _verticalStep = verticalStep;

            foreach (var enemy in _enemies)
            {
                var view = enemy.View;

                view.Collider.OnTriggerEnter2DAsObservable()
                    .Where(x => x.gameObject.layer == enemy.VulnerableBulletLayer)
                    .Subscribe(_ => HandleKilledEvent(enemy))
                    .AddTo(_disposable);
            }

            _isLeftDirection = false;
            _initialized = true;
        }

        void IDisposable.Dispose()
        {
            if (_enemies != null)
            {
                foreach (var enemy in _enemies.Where(enemy => enemy.View))
                {
                    Object.Destroy(enemy.View.gameObject);
                }

                _enemies.Clear();
            }

            _disposable.Dispose();
            _initialized = false;
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
            if (_isPaused || !_initialized)
            {
                return;
            }

            MoveHorizontal();
            HandleScreenCollisions();
        }

        private void HandleKilledEvent(EnemyModel model)
        {
            _currentSpeed += _speedStep;
            Debug.Log(model.Name + " killed");
        }

        private void MoveHorizontal()
        {
            var offset = _isLeftDirection ? Vector2.left : Vector2.right;

            DoWithEach(enemy => enemy.Position += offset * _currentSpeed);
        }

        private void HandleScreenCollisions()
        {
            float ppu = _canvasScaler.referencePixelsPerUnit;
            var boundsX = new Vector2(_canvasRect.xMin / ppu, _canvasRect.xMax / ppu);
            var boundsY = new Vector2(_canvasRect.yMin / ppu, _canvasRect.yMax / ppu);

            bool switchDirection = _enemies.Any(enemy => enemy.IsOutOfBoundsHorizontal(boundsX));

            if (switchDirection)
            {
                MoveVertical();
                _isLeftDirection = !_isLeftDirection;
            }

            bool outOfBounds = _enemies.Any(enemy => enemy.IsOutOfBoundsVertical(boundsY));

            if (outOfBounds)
            {
                _levelingService.Restart();
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