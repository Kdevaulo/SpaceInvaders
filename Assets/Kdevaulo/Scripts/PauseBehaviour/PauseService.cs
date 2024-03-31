using System.Collections.Generic;

using Kdevaulo.SpaceInvaders.MenuBehaviour;

using Zenject;

namespace Kdevaulo.SpaceInvaders.PauseBehaviour
{
    public sealed class PauseService
    {
        [Inject]
        private List<IPauseHandler> _pauseHandlers = new List<IPauseHandler>();

        [Inject]
        private PauseMenuView _pauseMenuView;

        public void Pause()
        {
            foreach (var handler in _pauseHandlers)
            {
                handler.HandlePause();
            }

            _pauseMenuView.SetActive(true);
        }

        public void Resume()
        {
            foreach (var handler in _pauseHandlers)
            {
                handler.HandleResume();
            }

            _pauseMenuView.SetActive(false);
        }
    }
}