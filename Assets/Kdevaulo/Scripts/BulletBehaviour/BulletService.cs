using System;
using System.Collections.Generic;

using UniRx;
using UniRx.Triggers;

using UnityEngine;

using Zenject;

using Object = UnityEngine.Object;

namespace Kdevaulo.SpaceInvaders.BulletBehaviour
{
    public sealed class BulletService : ITickable, IPauseHandler, IDisposable, IResourceHandler
    {
        [Inject]
        private BulletPool _bulletPool;

        private CompositeDisposable _disposable = new CompositeDisposable();
        private List<BulletModel> _activeBullets = new List<BulletModel>();

        private bool _isPaused;

        public void AddBullet(Vector2 direction, float speed, Vector2 startPosition, string shooterTag)
        {
            var view = _bulletPool.GetPooledObject();
            view.tag = shooterTag;
            var model = new BulletModel(view, direction, speed, startPosition);
            _activeBullets.Add(model);

            view.Collider.OnTriggerEnter2DAsObservable()
                .Where(x => !x.CompareTag(shooterTag))
                .Subscribe(_ => HandleCollision(model))
                .AddTo(_disposable);
        }

        void ITickable.Tick()
        {
            if (_isPaused) return;

            foreach (var bullet in _activeBullets)
            {
                bullet.Position += bullet.Speed * bullet.Direction;
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

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        void IResourceHandler.Release()
        {
            foreach (var bullet in _activeBullets)
            {
                Object.Destroy(bullet.View);
            }

            _activeBullets.Clear();
            _disposable.Clear();
        }

        private void HandleCollision(BulletModel model)
        {
            _bulletPool.ReturnToPool(model.View);
            _activeBullets.Remove(model);
        }
    }
}