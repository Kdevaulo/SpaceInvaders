using UnityEngine;
using UnityEngine.UI;

namespace Kdevaulo.SpaceInvaders.Menu
{
    [AddComponentMenu(nameof(PauseMenuView) + " in " + nameof(Menu))]
    public sealed class PauseMenuView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;
    }
}