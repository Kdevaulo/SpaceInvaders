using System;

using Kdevaulo.SpaceInvaders.BulletBehaviour;

using UniRx;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Zenject;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    public sealed class PlayerController : IPauseHandler, IDisposable, ITickable
    {
        [Inject]
        private BulletService _bulletService;
        
        [Inject]
        private Camera _camera;

        private PlayerModel _model;

        private bool _isPaused;
        private bool _isInitialized;

        private bool _canMove;

        private float _shootingRate;
        private float _currentTime;
        private float _bulletSpeed;

        public void Initialize(PlayerModel model)
        {
            _model = model;

            _shootingRate = _model.ShootingRate;
            _bulletSpeed = _model.BulletSpeed;

            _model.View.OnBeginDrag.AsObservable().Subscribe(_ => _canMove = true);
            _model.View.OnEndDrag.AsObservable().Subscribe(_ => _canMove = false);
            _model.View.OnDrag.AsObservable().Subscribe(TryMove);

            _isInitialized = true;
        }

        void IDisposable.Dispose()
        {
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

            TryShoot();
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

        private void Shoot()
        {
            _bulletService.AddBullet(_model.BulletDirection, _bulletSpeed, _model.Position, _model.PlayerTag);
        }

        private void TryMove(PointerEventData eventData)
        {
            if (_canMove || !_isPaused)
            {
                _model.Position = _camera.ScreenToWorldPoint(eventData.position);
            }
        }
    }
}