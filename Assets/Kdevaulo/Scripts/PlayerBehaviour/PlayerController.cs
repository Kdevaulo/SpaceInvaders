using System;

using Kdevaulo.SpaceInvaders.BulletBehaviour;

using UniRx;

using UnityEngine;
using UnityEngine.EventSystems;

using Zenject;

using Object = UnityEngine.Object;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    public sealed class PlayerController : IPauseHandler, IDisposable, ITickable, IResourceHandler
    {
        [Inject]
        private BulletService _bulletService;

        [Inject]
        private ScreenUtilities _screenUtilities;

        private CompositeDisposable _disposable = new CompositeDisposable();
        private PlayerModel _model;

        private bool _isPaused;
        private bool _isInitialized;

        private bool _canMove;

        private float _shootingRate;
        private float _currentTime;
        private float _bulletSpeed;

        private Rect _screenRect;
        private Vector2 _targetPosition;

        public void Initialize(PlayerModel model)
        {
            _model = model;

            _shootingRate = _model.ShootingRate;
            _bulletSpeed = _model.BulletSpeed;

            _screenRect = _screenUtilities.GetScreenRectInUnits();
            _targetPosition = _model.Position;

            _model.View.OnBeginDrag.AsObservable()
                .Subscribe(_ => _canMove = true)
                .AddTo(_disposable);

            _model.View.OnEndDrag.AsObservable()
                .Subscribe(_ => _canMove = false)
                .AddTo(_disposable);

            _model.View.OnDrag.AsObservable()
                .Subscribe(TryChangeTargetPosition)
                .AddTo(_disposable);

            _isInitialized = true;
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

            TryMove();
            TryShoot();
        }

        void IResourceHandler.Release()
        {
            Object.Destroy(_model.View.gameObject);
            _disposable.Clear();
            _isInitialized = false;
        }

        private void TryShoot()
        {
            if (_currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
            }
            else
            {
                Shoot();
                _currentTime = _shootingRate;
            }
        }

        private void TryMove()
        {
            _model.Position = Vector2.Lerp(_model.Position, _targetPosition, _model.MovementSpeed);
        }

        private void Shoot()
        {
            _bulletService.AddBullet(_model.BulletDirection, _bulletSpeed, _model.Position, _model.PlayerTag);
        }

        private void TryChangeTargetPosition(PointerEventData eventData)
        {
            if (_canMove || !_isPaused)
            {
                var eventDataPosition = _screenUtilities.ScreenToWorldPoint(eventData.position);

                bool isOutOfRect = _screenUtilities.IsOutOfRect(eventDataPosition, _model.HalfVerticalSize,
                    _model.HalfHorizontalSize, _screenRect);

                if (isOutOfRect) return;

                _targetPosition = eventDataPosition;
            }
        }
    }
}