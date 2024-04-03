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

        public readonly MovingItemView View;
        public readonly DropItem DropType;
        public readonly Enemy Name;

        public readonly int RewardPoints;
        public readonly string VulnerableProjectileTag;

        private readonly float HalfHorizontalSize;
        private readonly float HalfVerticalSize;

        private Vector2 _position;

        public EnemyModel(MovingItemView view, EnemySettings settings)
        {
            View = view;
            DropType = settings.DropType;
            Name = settings.Name;
            RewardPoints = settings.RewardPoints;
            VulnerableProjectileTag = settings.VulnerableProjectileTag;

            var bounds = view.Collider.bounds;
            HalfHorizontalSize = bounds.size.x / 2;
            HalfVerticalSize = bounds.size.y / 2;
        }

        public bool IsOutOfBoundsHorizontal(Rect rect)
        {
            return Position.x - HalfHorizontalSize < rect.xMin || Position.x + HalfHorizontalSize > rect.xMax;
        }

        public bool IsOutOfBoundsVertical(Rect rect)
        {
            return Position.y - HalfVerticalSize < rect.yMin || Position.y + HalfVerticalSize > rect.yMax;
        }
    }
}