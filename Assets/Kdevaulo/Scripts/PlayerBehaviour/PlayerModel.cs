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

        public readonly string VulnerableProjectileTag;
        public readonly string PlayerTag;
        public readonly float ShootingRate;
        public readonly float BulletSpeed;
        public readonly Vector2 BulletDirection;

        private Vector2 _position;

        public PlayerModel(MovingItemView view, PlayerSettings settings)
        {
            View = view;
            Position = settings.StartPosition;
            VulnerableProjectileTag = settings.VulnerableProjectileTag;
            ShootingRate = settings.ShootingRate;
            BulletSpeed = settings.BulletSpeed;
            BulletDirection = settings.BulletDirection;
            PlayerTag = View.tag;
        }
    }
}