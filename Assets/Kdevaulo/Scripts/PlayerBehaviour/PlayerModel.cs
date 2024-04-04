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

        public readonly DraggableItemView View;

        public readonly string VulnerableProjectileTag;
        public readonly string PlayerTag;
        public readonly string BulletTag;

        public readonly float ShootingDelay;
        public readonly float MovementDelay;
        public readonly float HalfVerticalSize;
        public readonly float MovementSmoothness;
        public readonly float HalfHorizontalSize;

        public readonly Vector2 BulletDirection;

        private Vector2 _position;

        public PlayerModel(DraggableItemView view, PlayerSettings settings)
        {
            View = view;
            Position = settings.StartPosition;
            ShootingDelay = settings.ShootingDelay;
            MovementDelay = settings.MovementDelay;
            BulletDirection = settings.BulletDirection;
            MovementSmoothness = settings.MovementSmoothness;
            VulnerableProjectileTag = settings.VulnerableProjectileTag;

            HalfVerticalSize = View.VerticalSize / 2;
            HalfHorizontalSize = View.HorizontalSize / 2;

            PlayerTag = View.tag;
            BulletTag = settings.BulletTag;
        }
    }
}