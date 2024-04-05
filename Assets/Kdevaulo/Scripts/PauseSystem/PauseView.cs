using UnityEngine;
using UnityEngine.UI;

namespace Kdevaulo.SpaceInvaders.PauseSystem
{
    [AddComponentMenu(nameof(PauseView) + " in " + nameof(PauseSystem))]
    public sealed class PauseView : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public Button PauseButton { get; private set; }
    }
}