using System;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    [Serializable]
    public sealed class PlayerSettings
    {
        public DraggableItemView View;
        public Vector2 StartPosition;
        public Vector2 BulletDirection;
        public string VulnerableProjectileTag;

        [Min(0.00001f)]
        public float ShootingRate;
        [Min(0.00001f)]
        public float BulletSpeed;
        [Min(0.00001f)]
        public float MovementSpeed;
    }
}