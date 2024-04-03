using UnityEngine;

namespace Kdevaulo.SpaceInvaders.BulletBehaviour
{
    public sealed class BulletModel
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
        public readonly Vector2 Direction;

        public readonly float Speed;

        private readonly float HalfHorizontalSize;
        private readonly float HalfVerticalSize;

        private Vector2 _position;

        public BulletModel(MovingItemView view, Vector2 direction, float speed, Vector2 startPosition)
        {
            View = view;
            Direction = direction;
            Speed = speed;
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