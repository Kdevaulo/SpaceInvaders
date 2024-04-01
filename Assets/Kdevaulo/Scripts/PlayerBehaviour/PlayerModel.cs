using UnityEngine;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    public sealed class PlayerModel
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

        public readonly int VulnerableBulletLayer;

        private Vector2 _position;

        public PlayerModel(MovingItemView view, PlayerSettings settings)
        {
            View = view;
            Position = settings.StartPosition;
            VulnerableBulletLayer = settings.VulnerableProjectileLayer;
        }
    }
}