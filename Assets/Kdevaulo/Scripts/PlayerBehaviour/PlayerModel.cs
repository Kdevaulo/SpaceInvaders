using UnityEngine;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    public sealed class PlayerModel : BaseModel<DraggableItemView>
    {
        public readonly string[] VulnerableObjectsTags;

        public readonly string PlayerTag;
        public readonly string BulletTag;

        public readonly float ShootingDelay;
        public readonly float MovementDelay;
        public readonly float MovementSmoothness;

        public readonly Vector2 BulletDirection;

        public PlayerModel(DraggableItemView view, PlayerSettingsData settings) : base(view)
        {
            Position = settings.StartPosition;
            ShootingDelay = settings.ShootingDelay;
            MovementDelay = settings.MovementDelay;
            BulletDirection = settings.BulletDirection;
            MovementSmoothness = settings.MovementSmoothness;
            VulnerableObjectsTags = settings.VulnerableObjectsTags;

            HalfVerticalSize = View.VerticalSize / 2;
            HalfHorizontalSize = View.HorizontalSize / 2;

            PlayerTag = View.tag;
            BulletTag = settings.BulletTag;
        }
    }
}