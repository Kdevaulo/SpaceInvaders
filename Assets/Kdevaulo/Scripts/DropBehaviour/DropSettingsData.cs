using UnityEngine;

namespace Kdevaulo.SpaceInvaders.DropBehaviour
{
    [CreateAssetMenu(fileName = nameof(DropSettingsData),
        menuName = nameof(DropBehaviour) + "/" + nameof(DropSettingsData))]
    public sealed class DropSettingsData : ScriptableObject
    {
        [field: SerializeField] public DropItem[] Drops { get; private set; }
    }
}