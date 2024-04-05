using UnityEngine;

namespace Kdevaulo.SpaceInvaders
{
    public class BaseModel<T> where T : MovingItemView
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

        public float HalfVerticalSize { get; protected set; }
        public float HalfHorizontalSize { get; protected set; }

        public readonly T View;

        private Vector2 _position;

        public BaseModel(T view)
        {
            View = view;
        }
    }
}