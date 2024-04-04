using System;

using Kdevaulo.SpaceInvaders.BulletBehaviour;
using Kdevaulo.SpaceInvaders.LevelSystem;
using Kdevaulo.SpaceInvaders.ScoreBehaviour;

using UniRx;
using UniRx.Triggers;

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
        private LevelingService _levelingService;

        [Inject]
        private ScoreService _scoreService;

        [Inject]
        private ScreenUtilities _screenUtilities;

        private CompositeDisposable _disposable = new CompositeDisposable();

        private PlayerModel _model;

        private bool _canMove;
        private bool _isPaused;
        private bool _isInitialized;

        private float _shootingDelay;
        private float _shootTimeCounter;
        private float _movingDelay;
        private float _moveTimeCounter;

        private Rect _screenRect;
        private Vector2 _targetPosition;

        public void Initialize(PlayerModel model)
        {
            _model = model;

            _targetPosition = _model.Position;
            _movingDelay = _model.MovementDelay;
            _shootingDelay = _model.ShootingDelay;

            _screenRect = _screenUtilities.GetScreenRectInUnits();

            _model.View.Collider.OnTriggerEnter2DAsObservable()
                .Where(x => x.CompareTag(_model.VulnerableProjectileTag))
                .Subscribe(_ =>
                {
                    _levelingService.Restart();
                    _scoreService.Clear();
                })
                .AddTo(_disposable);

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
            _shootTimeCounter -= Time.deltaTime;

            if (_shootTimeCounter <= 0)
            {
                Shoot();
                _shootTimeCounter = _shootingDelay;
            }
        }

        private void TryMove()
        {
            _moveTimeCounter -= Time.deltaTime;

            if (_moveTimeCounter <= 0)
            {
                _model.Position = Vector2.Lerp(_model.Position, _targetPosition, _model.MovementSmoothness);
                _moveTimeCounter = _movingDelay;
            }
        }

        private void Shoot()
        {
            _bulletService.AddBullet(_model.BulletDirection, _model.Position, _model.PlayerTag,
                _model.BulletTag);
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