using System.Collections.Generic;

using Kdevaulo.Interfaces;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders.PauseBehaviour
{
    public sealed class PauseService
    {
        private List<IPauseHandler> _pauseHandlers = new List<IPauseHandler>();

        public void Pause()
        {
            foreach (var handler in _pauseHandlers)
            {
                handler.HandlePause();
            }
        }

        public void Resume()
        {
            foreach (var handler in _pauseHandlers)
            {
                handler.HandleResume();
            }
        }

        public void AddPauseHandler(IPauseHandler handler)
        {
            _pauseHandlers.Add(handler);
        }

        public void RemovePauseHandler(IPauseHandler handler)
        {
            if (_pauseHandlers.Contains(handler))
            {
                _pauseHandlers.Remove(handler);
            }
            else
            {
                Debug.LogError(
                    $"{nameof(PauseService)} {nameof(RemovePauseHandler)} — Trying to remove handler that doesn't exist");
            }
        }
    }
}