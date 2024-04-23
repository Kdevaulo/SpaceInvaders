using System;
using System.Collections.Generic;

using Kdevaulo.SpaceInvaders.ScreenSystem;

using UniRx;
using UniRx.Triggers;

using UnityEngine;

using Zenject;

namespace Kdevaulo.SpaceInvaders.ProjectileBehaviour
{
    public sealed class ProjectileService : IInitializable, ITickable, IPauseHandler, IDisposable, IResourceHandler
    {
        bool IPauseHandler.IsPaused
        {
            set => _isPaused = value;
        }

        [Inject] private ScreenService _screenService;

        [Inject] private ProjectilePool _projectilePool;

        [Inject] private ProjectileSettingsData _bulletSettingsData;

        private CompositeDisposable _disposable = new CompositeDisposable();

        private List<ProjectileModel> _activeBullets = new List<ProjectileModel>();
        private List<ProjectileModel> _bulletsToReturn = new List<ProjectileModel>();

        private Dictionary<ProjectileModel, IDisposable> _disposableByModel =
            new Dictionary<ProjectileModel, IDisposable>();

        private float _moveDelay;
        private float _timeCounter;
        private float _moveStepDivider;

        private bool _isPaused;

        private Rect _screenRect;

        public void AddBullet(Vector2 direction, Vector2 startPosition, string[] ignoreTags, string bulletTag)
        {
            var view = _projectilePool.GetPooledObject();
            var model = new ProjectileModel(view, direction, startPosition);

            view.tag = bulletTag;

            var disposable = view.Collider.OnTriggerEnter2DAsObservable()
                .Where(x => !x.IfAnyTag(ignoreTags))
                .Subscribe(_ => ReturnBullet(model))
                .AddTo(_disposable);

            _activeBullets.Add(model);
            _disposableByModel.Add(model, disposable);
        }

        void IInitializable.Initialize()
        {
            _moveDelay = _bulletSettingsData.MoveDelay;
            _screenRect = _screenService.GetScreenRectInUnits();
            _moveStepDivider = _bulletSettingsData.MoveStepDivider;

            _timeCounter = _moveDelay;
        }

        void ITickable.Tick()
        {
            if (_isPaused || _activeBullets.Count == 0) return;

            _timeCounter -= Time.deltaTime;

            if (_timeCounter <= 0)
            {
                foreach (var bullet in _activeBullets)
                {
                    bullet.Position += bullet.Direction / _moveStepDivider;
                    HandleScreenCollisions(bullet);
                }

                foreach (var bullet in _bulletsToReturn)
                {
                    ReturnBullet(bullet);
                }

                _bulletsToReturn.Clear();

                _timeCounter = _moveDelay;
            }
        }

        void IDisposable.Dispose()
        {
            foreach (var disposable in _disposableByModel)
            {
                disposable.Value.Dispose();
            }

            _disposable.Dispose();
        }

        void IResourceHandler.Release()
        {
            _bulletsToReturn.AddRange(_activeBullets);

            foreach (var bullet in _bulletsToReturn)
            {
                ReturnBullet(bullet);
            }

            _disposable.Clear();
            _activeBullets.Clear();
            _bulletsToReturn.Clear();
        }

        private void HandleScreenCollisions(ProjectileModel model)
        {
            bool isOutOfBounds = Utilities.IsOutOfBoundsVertical(model.Position.y, model.HalfVerticalSize, _screenRect);

            if (isOutOfBounds)
            {
                _bulletsToReturn.Add(model);
            }
        }

        private void ReturnBullet(ProjectileModel model)
        {
            _projectilePool.ReturnToPool(model.View);
            _activeBullets.Remove(model);

            _disposableByModel[model].Dispose();
            _disposableByModel.Remove(model);
        }
    }
}