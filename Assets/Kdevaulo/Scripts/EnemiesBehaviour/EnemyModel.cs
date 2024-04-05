using Kdevaulo.SpaceInvaders.DropBehaviour;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders.EnemiesBehaviour
{
    public sealed class EnemyModel : BaseModel<MovingItemView>
    {
        public Vector2Int Index { get; set; }

        public readonly Drop DropType;

        public readonly int RewardPoints;

        public readonly string[] VulnerableObjectsTags;

        public EnemyModel(MovingItemView view, EnemySettings settings) : base(view)
        {
            DropType = settings.DropType;
            RewardPoints = settings.RewardPoints;
            VulnerableObjectsTags = settings.VulnerableObjectsTags;

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