using UnityEngine;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    [CreateAssetMenu(fileName = nameof(PlayerSettings),
        menuName = nameof(PlayerBehaviour) + "/" + nameof(PlayerSettings))]
    public sealed class PlayerSettings : ScriptableObject
    {
        public DraggableItemView View;

        public Vector2 StartPosition;
        public Vector2 BulletDirection;

        public string BulletTag;
        public string[] VulnerableObjectsTags;

        [Min(0.00001f)]
        public float ShootingDelay;
        [Min(0.00001f)]
        public float MovementSmoothness;
        [Min(0.00001f)]
        public float MovementDelay;
    }
}