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

        private Vector2 _position;

        public BulletModel(MovingItemView view, Vector2 direction, float speed, Vector2 startPosition)
        {
            View = view;
            Direction = direction;
            Speed = speed;
            Position = startPosition;
        }
    }
}