using System.Collections.Generic;

using Zenject;

namespace Kdevaulo.SpaceInvaders.PauseSystem
{
    public sealed class PauseService
    {
        [Inject] private List<IPauseHandler> _pauseHandlers = new List<IPauseHandler>();

        [Inject] private PauseMenuView _pauseMenuView;

        public void Pause()
        {
            foreach (var handler in _pauseHandlers)
            {
                handler.IsPaused = true;
            }

            _pauseMenuView.SetActive(true);
        }

        public void Resume()
        {
            foreach (var handler in _pauseHandlers)
            {
                handler.IsPaused = false;
            }

            _pauseMenuView.SetActive(false);
        }
    }
}