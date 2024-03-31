using Kdevaulo.SpaceInvaders.DropBehaviour;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders.EnemiesBehaviour
{
    public sealed class EnemyModel
    {
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                View.SetPosition(_position);
            }
        }

        public readonly EnemyView View;
        public readonly DropItem DropType;
        public readonly Enemy Name;

        public readonly int RewardPoints;
        public readonly int VulnerableBulletLayer;

        private readonly float HalfHorizontalSize;
        private readonly float HalfVerticalSize;

        private Vector2 _position;

        public EnemyModel(EnemyView view, EnemySettings settings)
        {
            View = view;
            DropType = settings.DropType;
            Name = settings.Name;
            RewardPoints = settings.RewardPoints;
            VulnerableBulletLayer = settings.VulnerableProjectileLayer;

            var bounds = view.Collider.bounds;
            HalfHorizontalSize = bounds.size.x / 2;
            HalfVerticalSize = bounds.size.y / 2;
        }

        public bool IsOutOfBoundsHorizontal(Vector2 boundsX)
        {
            return Position.x - HalfHorizontalSize < boundsX.x || Position.x + HalfHorizontalSize > boundsX.y;
        }

        public bool IsOutOfBoundsVertical(Vector2 boundsY)
        {
            return Position.y - HalfVerticalSize < boundsY.x || Position.y + HalfVerticalSize > boundsY.y;
        }
    }
}