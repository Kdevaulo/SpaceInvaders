using System;

using Kdevaulo.SpaceInvaders.MenuBehaviour;
using Kdevaulo.SpaceInvaders.PauseBehaviour;
using Kdevaulo.SpaceInvaders.ScoreBehaviour;

using UniRx;

using Zenject;

namespace Kdevaulo.SpaceInvaders.LevelSystem
{
    public sealed class LevelController : IInitializable, IDisposable
    {
        [Inject]
        private LevelingService _levelingService;

        [Inject]
        private ScoreService _scoreService;

        [Inject]
        private PauseService _pauseService;

        private CompositeDisposable _disposable = new CompositeDisposable();

        public LevelController(PauseMenuView pauseMenu)
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