using System;
using System.Collections.Generic;

using UniRx;
using UniRx.Triggers;

using UnityEngine;

using Zenject;

namespace Kdevaulo.SpaceInvaders.BulletBehaviour
{
    public sealed class BulletService : IInitializable, ITickable, IPauseHandler, IDisposable, IResourceHandler
    {
        [Inject]
        private BulletPool _bulletPool;

        [Inject]
        private ScreenUtilities _screenUtilities;

        private CompositeDisposable _disposable = new CompositeDisposable();

        private List<BulletModel> _activeBullets = new List<BulletModel>();
        private List<BulletModel> _bulletsToReturn = new List<BulletModel>();

        private Dictionary<BulletModel, IDisposable> _disposableByModel = new Dictionary<BulletModel, IDisposable>();

        private bool _isPaused;
        private Rect _screenRect;

        public void AddBullet(Vector2 direction, float speed, Vector2 startPosition, string shooterTag, string bulletTag)
        {
            var view = _bulletPool.GetPooledObject();
            view.tag = bulletTag;
            var model = new BulletModel(view, direction, speed, startPosition);
            _activeBullets.Add(model);

            var disposable = view.Collider.OnTriggerEnter2DAsObservable()
                .Where(x => !x.CompareTag(shooterTag))
                .Subscribe(_ => { ReturnBullet(model); })
                .AddTo(_disposable);

            _disposableByModel.Add(model, disposable);
        }

        void IInitializable.Initialize()
        {
            _screenRect = _screenUtilities.GetScreenRectInUnits();
        }

        void ITickable.Tick()
        {
            if (_isPaused) return;

            foreach (var bullet in _activeBullets)
            {
                bullet.Position += bullet.Speed * bullet.Direction;
                HandleScreenCollisions(bullet);
            }

            foreach (var bullet in _bulletsToReturn)
            {
                ReturnBullet(bullet);
            }

            _bulletsToReturn.Clear();
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

            _activeBullets.Clear();
            _bulletsToReturn.Clear();
            _disposable.Clear();
        }

        private void HandleScreenCollisions(BulletModel model)
        {
            if (model.IsOutOfBoundsVertical(_screenRect) || model.IsOutOfBoundsHorizontal(_screenRect))
            {
                _bulletsToReturn.Add(model);
            }
        }

        private void ReturnBullet(BulletModel model)
        {
            _bulletPool.ReturnToPool(model.View);
            _activeBullets.Remove(model);

            _disposableByModel[model].Dispose();
            _disposableByModel.Remove(model);
        }
    }
}