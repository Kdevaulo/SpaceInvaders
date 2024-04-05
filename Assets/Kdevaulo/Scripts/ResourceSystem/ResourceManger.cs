using System;

using Kdevaulo.SpaceInvaders.LevelngSystem;

using UniRx;

using Zenject;

namespace Kdevaulo.SpaceInvaders.ResourceSystem
{
    public sealed class ResourceManger : IInitializable, IDisposable
    {
        [Inject] private IResourceHandler[] _resourceHandlers;

        [Inject] private LevelingModel _levelingModel;

        private CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _levelingModel.ClearLevel
                .Subscribe(_ => ReleaseResources())
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        private void ReleaseResources()
        {
            foreach (var handler in _resourceHandlers)
            {
                handler.Release();
            }
        }
    }
}