using UnityEngine;

namespace Kdevaulo.SpaceInvaders.BulletBehaviour
{
    public sealed class BulletModel : BaseModel<MovingItemView>
    {
        public readonly Vector2 Direction;

        public BulletModel(MovingItemView view, Vector2 direction, Vector2 startPosition) : base(view)
        {
            Direction = direction;
            Position = startPosition;

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