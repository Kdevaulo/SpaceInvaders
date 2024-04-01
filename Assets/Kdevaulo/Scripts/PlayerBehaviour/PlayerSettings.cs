using System;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    [Serializable]
    public sealed class PlayerSettings
    {
        public MovingItemView View;
        public Vector2 StartPosition;
        public int VulnerableProjectileLayer;
    }
}