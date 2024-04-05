using System;

using Kdevaulo.SpaceInvaders.PauseSystem;
using Kdevaulo.SpaceInvaders.ScoreSystem;

using UniRx;

using Zenject;

namespace Kdevaulo.SpaceInvaders.LevelngSystem
{
    public sealed class LevelingController : IInitializable, IDisposable
    {
        [Inject] private ScoreService _scoreService;
        [Inject] private PauseService _pauseService;
        [Inject] private LevelingService _levelingService;

        private CompositeDisposable _disposable = new CompositeDisposable();

        public LevelingController(PauseMenuView pauseMenu)
        {
            pauseMenu.RestartButton.OnClickAsObservable().Subscribe(_ => Restart()).AddTo(_disposable);
        }

        void IInitializable.Initialize()
        {
            _levelingService.Initialize();
        }

        private void Restart()
        {
            _levelingService.Restart();
            _scoreService.Clear();
            _pauseService.Resume();
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}