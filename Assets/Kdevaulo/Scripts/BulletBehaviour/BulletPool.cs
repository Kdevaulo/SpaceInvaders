using System;
using System.Collections.Generic;

using Zenject;

using Object = UnityEngine.Object;

namespace Kdevaulo.SpaceInvaders.BulletBehaviour
{
    public sealed class BulletPool : IInitializable, IDisposable, IResourceHandler
    {
        [Inject]
        private MovingItemView _objectToPool;

        private Stack<MovingItemView> _stack;

        private uint _initialPoolSize = 3;

        public MovingItemView GetPooledObject()
        {
            if (_objectToPool == null)
            {
                return null;
            }

            if (_stack.Count == 0)
            {
                return Object.Instantiate(_objectToPool);
            }

            var nextInstance = _stack.Pop();
            nextInstance.gameObject.SetActive(true);
            return nextInstance;
        }

        public void ReturnToPool(MovingItemView pooledObject)
        {
            _stack.Push(pooledObject);
            pooledObject.gameObject.SetActive(false);
        }

        void IDisposable.Dispose()
        {
            _stack.Clear();
        }

        void IInitializable.Initialize()
        {
            if (_objectToPool == null)
            {
                return;
            }

            _stack = new Stack<MovingItemView>();

            for (int i = 0; i < _initialPoolSize; i++)
            {
                var instance = Object.Instantiate(_objectToPool);
                instance.gameObject.SetActive(false);
                _stack.Push(instance);
            }
        }

        void IResourceHandler.Release()
        {
            foreach (var item in _stack)
            {
                Object.Destroy(item.gameObject);
            }

            _stack.Clear();
        }
    }
}