using System;
using System.Collections.Generic;
using System.Linq;

using Kdevaulo.Interfaces;

using UniRx;
using UniRx.Triggers;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Kdevaulo.SpaceInvaders.EnemiesBehaviour
{
    public sealed class EnemiesController : ITickable, IPauseHandler, IDisposable
    {
        [Inject]
        private Rect _canvasRect;
        [Inject]
        private CanvasScaler _canvasScaler;

        private List<EnemyModel> _enemies;

        private CompositeDisposable _disposable = new CompositeDisposable();

        private bool _isLeftDirection;
        private bool _isPaused;

        private float _currentSpeed;
        private float _speedStep;

        public void Initialize(List<EnemyModel> enemies, float startSpeed, float speedStep)
        {
            _enemies = enemies;
            _currentSpeed = startSpeed;
            _speedStep = speedStep;

            foreach (var enemy in _enemies)
            {
                var view = enemy.View;

                view.Collider.OnTriggerEnter2DAsObservable()
                    .Where(x => x.gameObject.layer == enemy.VulnerableBulletLayer)
                    .Subscribe(_ => HandleKilledEvent(enemy))
                    .AddTo(_disposable);
            }
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
            if (_isPaused)
            {
                return;
            }

            Move();
            HandleScreenCollisions();
        }

        private void Move()
        {
            var offset = _isLeftDirection ? Vector2.left : Vector2.right;

            DoWithEach(enemy => enemy.Position += offset * _currentSpeed);
        }

        private void HandleScreenCollisions()
        {
            float ppu = _canvasScaler.referencePixelsPerUnit;
            var boundsX = new Vector2(_canvasRect.xMin / ppu, _canvasRect.xMax / ppu);

            bool switchDirection = _enemies.Any(enemy => enemy.IsOutOfBounds(boundsX));

            if (switchDirection)
            {
                _isLeftDirection = !_isLeftDirection;
            }
        }

        private void HandleKilledEvent(EnemyModel model)
        {
            _currentSpeed += _speedStep;
            Debug.Log(model.Name + " killed");
        }

        private void DoWithEach(Action<EnemyModel> action)
        {
            foreach (var enemy in _enemies)
            {
                action.Invoke(enemy);
            }
        }

        // todo: call Dispose at destroy
        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}