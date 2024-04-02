using System;

using Kdevaulo.SpaceInvaders.BulletBehaviour;

using UnityEngine;

using Zenject;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    public sealed class PlayerController : IPauseHandler, IDisposable, ITickable
    {
        [Inject]
        private BulletService _bulletService;

        private PlayerModel _model;

        private bool _isPaused;
        private bool _isInitialized;

        private float _shootingRate;
        private float _currentTime;
        private float _bulletSpeed;

        public void Initialize(PlayerModel model)
        {
            _model = model;

            _shootingRate = _model.ShootingRate;
            _bulletSpeed = _model.BulletSpeed;

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
    }
}