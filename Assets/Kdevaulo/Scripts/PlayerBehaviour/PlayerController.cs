using System;

using Zenject;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    public sealed class PlayerController : IPauseHandler, IDisposable, ITickable
    {
        private PlayerModel _model;

        private bool _isPaused;
        private bool _isInitialized;

        public void Initialize(PlayerModel model)
        {
            _model = model;
            _isInitialized = true;
        }

        void IDisposable.Dispose()
        {
            _isInitialized = false;
        }

        void IPauseHandler.HandlePause()
        {
            _isPaused = true;
        }

        void IPauseHandler.HandleResume()
        {
            _isPaused = false;
        }

        void ITickable.Tick()
        {
            if (_isPaused || !_isInitialized)
            {
                return;
            }
        }
    }
}