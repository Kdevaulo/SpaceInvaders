using System;

using Kdevaulo.SpaceInvaders.MenuBehaviour;

using UniRx;

using Zenject;

namespace Kdevaulo.SpaceInvaders.LevelSystem
{
    public sealed class LevelController : IInitializable, IDisposable
    {
        [Inject]
        private LevelingService _levelingService;

        private CompositeDisposable _disposable = new CompositeDisposable();

        public LevelController(PauseMenuView pauseMenu)
        {
            pauseMenu.RestartButton.OnClickAsObservable().Subscribe(_ => Restart()).AddTo(_disposable);
        }

        public void StartNextLevel()
        {
            _levelingService.StartNewStage();
        }

        void IInitializable.Initialize()
        {
            StartFromTheBeginning();
        }

        private void Restart()
        {
            _levelingService.Restart();
        }

        private void StartFromTheBeginning()
        {
            _levelingService.StartFromTheBeginning();
        }

        //todo: dispose on destroy
        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}