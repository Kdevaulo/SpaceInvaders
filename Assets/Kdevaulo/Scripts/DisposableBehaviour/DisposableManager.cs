using System;
using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Kdevaulo.SpaceInvaders.DisposableBehaviour
{
    [AddComponentMenu(nameof(DisposableManager) + " in " + nameof(DisposableBehaviour))]
    public sealed class DisposableManager : MonoBehaviour
    {
        [Inject]
        private List<IDisposable> _disposables = new List<IDisposable>();

        public void DisposeAll()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables.Clear();
        }
    }
}