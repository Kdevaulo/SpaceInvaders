using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Kdevaulo.SpaceInvaders.BulletBehaviour
{
    public sealed class BulletPool : IInitializable, IResourceHandler
    {
        [Inject]
        private MovingItemView _objectToPool;

        private Stack<MovingItemView> _stack;

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

        void IInitializable.Initialize()
        {
            _stack = new Stack<MovingItemView>();
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