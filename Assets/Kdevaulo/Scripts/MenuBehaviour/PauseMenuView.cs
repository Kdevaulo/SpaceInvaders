using UnityEngine;
using UnityEngine.UI;

namespace Kdevaulo.SpaceInvaders.MenuBehaviour
{
    [AddComponentMenu(nameof(PauseMenuView) + " in " + nameof(MenuBehaviour))]
    public sealed class PauseMenuView : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public Button RestartButton { get; private set; }
        [field: SerializeField] public Button ContinueButton { get; private set; }
    }
}