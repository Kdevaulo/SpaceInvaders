using Zenject;

namespace Kdevaulo.SpaceInvaders
{
    public sealed class ScreenAspectHandler : IInitializable
    {
        [Inject]
        private ScreenUtilities _screenUtilities;

        void IInitializable.Initialize()
        {
            _screenUtilities.UpdateCameraSize();
        }
    }
}