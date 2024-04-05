using System;

using Kdevaulo.SpaceInvaders.LevelngSystem;
using Kdevaulo.SpaceInvaders.ProjectileBehaviour;
using Kdevaulo.SpaceInvaders.ScoreSystem;
using Kdevaulo.SpaceInvaders.ScreenSystem;

using UniRx;
using UniRx.Triggers;

using UnityEngine;
using UnityEngine.EventSystems;

using Zenject;

using Object = UnityEngine.Object;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    public sealed class PlayerController : IInitializable, IPauseHandler, IDisposable, ITickable, IResourceHandler
    {
        [Inject] private ScoreService _scoreService;
        [Inject] private ScreenService _screenService;
        [Inject] private LevelingService _levelingService;
        [Inject] private ProjectileService _projectileService;

        [Inject] private LevelingModel _levelingModel;

        [Inject] private PlayerSettingsData _playerSettings;

        private CompositeDisposable _eventsDisposable = new CompositeDisposable();
        private CompositeDisposable _collisionDisposable = new CompositeDisposable();

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

            TryMove();
            TryShoot();
        }

        void IPauseHandler.HandlePause()
        {
            _isPaused = true;
        }

        void IPauseHandler.HandleResume()
        {
            _isPaused = false;
        }

        void IDisposable.Dispose()
        {
            _collisionDisposable.Dispose();
            _eventsDisposable.Dispose();
            _isInitialized = false;
        }

        void IResourceHandler.Release()
        {
            Object.Destroy(_model.View.gameObject);
            _collisionDisposable.Clear();
            _isInitialized = false;
        }

        private void Prepare()
        {
            _model = CreatePlayer(_playerSettings);

            InitializeFields();
            SubscribeEvents();

            _isInitialized = true;
        }

        private PlayerModel CreatePlayer(PlayerSettingsData settings)
        {
            var view = Object.Instantiate(settings.View);
            var model = new PlayerModel(view, settings);
            return model;
        }

        private void InitializeFields()
        {
            _targetPosition = _model.Position;
            _movingDelay = _model.MovementDelay;
            _shootingDelay = _model.ShootingDelay;

            _screenRect = _screenService.GetScreenRectInUnits();
        }

        private void SubscribeEvents()
        {
            _model.View.Collider.OnTriggerEnter2DAsObservable()
                .Where(x => x.IfAnyTag(_model.VulnerableObjectsTags))
                .Subscribe(_ =>
                {
                    _levelingService.Restart();
                    _scoreService.Clear();
                })
                .AddTo(_collisionDisposable);

            _model.View.OnDrag.Subscribe(TryChangeTargetPosition).AddTo(_collisionDisposable);
            _model.View.OnEndDrag.Subscribe(_ => _canMove = false).AddTo(_collisionDisposable);
            _model.View.OnBeginDrag.Subscribe(_ => _canMove = true).AddTo(_collisionDisposable);
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

        private void TryShoot()
        {
            _shootTimeCounter -= Time.deltaTime;

            if (_shootTimeCounter <= 0)
            {
                Shoot();
                _shootTimeCounter = _shootingDelay;
            }
        }

        private void Shoot()
        {
            // todo: remove ""
            _projectileService.AddBullet(_model.BulletDirection, _model.Position,
                new string[] { _model.PlayerTag, "Drop" },
                _model.BulletTag);
        }

        private void TryChangeTargetPosition(PointerEventData eventData)
        {
            if (_canMove || !_isPaused)
            {
                var eventDataPosition = _screenService.ScreenToWorldPoint(eventData.position);

                bool isOutOfRect = Utilities.IsOutOfRect(eventDataPosition, _model.HalfVerticalSize,
                    _model.HalfHorizontalSize, _screenRect);

                if (isOutOfRect) return;

                _targetPosition = eventDataPosition;
            }
        }
    }
}