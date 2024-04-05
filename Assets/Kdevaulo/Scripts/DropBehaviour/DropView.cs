using UnityEngine;

namespace Kdevaulo.SpaceInvaders.DropBehaviour
{
    [AddComponentMenu(nameof(DropView) + " in " + nameof(DropBehaviour))]
    public sealed class DropView : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public Collider2D Collider { get; private set; }
    }
}