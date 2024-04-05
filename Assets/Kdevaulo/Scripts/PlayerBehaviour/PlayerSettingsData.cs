using UnityEngine;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    [CreateAssetMenu(fileName = nameof(PlayerSettingsData),
        menuName = nameof(PlayerBehaviour) + "/" + nameof(PlayerSettingsData))]
    public sealed class PlayerSettingsData : ScriptableObject
    {
        [field: Header("Values")]
        [field: Min(0.00001f)]
        [field: SerializeField] public float ShootingDelay { get; private set; }
        [field: Min(0.00001f)]
        [field: SerializeField] public float MovementDelay { get; private set; }
        [field: Min(0.00001f)]
        [field: SerializeField] public float MovementSmoothness { get; private set; }
        [field: SerializeField] public Vector2 StartPosition { get; private set; }
        [field: SerializeField] public Vector2 BulletDirection { get; private set; }

        [field: SerializeField] public string ProjectileTag { get; private set; }

        [field: SerializeField] public string[] ProjectileIgnoreTags { get; private set; }
        [field: SerializeField] public string[] VulnerableObjectsTags { get; private set; }

        [field: Header("References")]
        [field: SerializeField] public DraggableItemView View { get; private set; }
    }
}