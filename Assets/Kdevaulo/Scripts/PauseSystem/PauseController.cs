using System;

using UniRx;

using Zenject;

namespace Kdevaulo.SpaceInvaders.PauseSystem
{
    public sealed class PauseController : IDisposable
    {
        [Inject] private PauseService _pauseService;

        private CompositeDisposable _disposable = new CompositeDisposable();

        public PauseController(PauseView pauseView, PauseMenuView pauseMenu)
        {
            pauseView.PauseButton.OnClickAsObservable().Subscribe(_ => Pause()).AddTo(_disposable);
            pauseMenu.ContinueButton.OnClickAsObservable().Subscribe(_ => Resume()).AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        private void Pause()
        {
            _pauseService.Pause();
        }

        private void Resume()
        {
            _pauseService.Resume();
        }
    }
}