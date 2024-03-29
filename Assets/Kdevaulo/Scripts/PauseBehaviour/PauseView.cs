using UnityEngine;
using UnityEngine.UI;

namespace Kdevaulo.SpaceInvaders.PauseBehaviour
{
    [AddComponentMenu(nameof(PauseView) + " in " + nameof(PauseBehaviour))]
    public sealed class PauseView : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public Button PauseButton { get; private set; }
    }
}