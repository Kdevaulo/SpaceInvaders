using UnityEngine;

namespace Kdevaulo.SpaceInvaders.DropBehaviour
{
    [CreateAssetMenu(fileName = nameof(DropSettings), menuName = nameof(DropBehaviour) + "/" + nameof(DropSettings))]
    public sealed class DropSettings : ScriptableObject
    {
        [field: SerializeField] public DropItem[] Drops { get; private set; }
    }
}