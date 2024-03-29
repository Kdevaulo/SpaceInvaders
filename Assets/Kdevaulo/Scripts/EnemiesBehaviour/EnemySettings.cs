using System;

using Kdevaulo.SpaceInvaders.DropBehaviour;

namespace Kdevaulo.SpaceInvaders.EnemiesBehaviour
{
    [Serializable]
    public sealed class EnemySettings
    {
        public Enemy Name;
        public EnemyView View;
        public DropItem DropType;
        public int Count;
        public int RewardPoints;
        public int VulnerableProjectileLayer;
    }
}