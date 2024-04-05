using UnityEngine;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    public sealed class PlayerModel : BaseModel<DraggableItemView>
    {
        public readonly string[] ProjectileIgnoreTags;
        public readonly string[] VulnerableObjectsTags;

        public readonly string ProjectileTag;

        public readonly float ShootingDelay;
        public readonly float MovementDelay;
        public readonly float MovementSmoothness;

        public readonly Vector2 BulletDirection;

        public PlayerModel(DraggableItemView view, PlayerSettingsData settings) : base(view)
        {
            Position = settings.StartPosition;
            ProjectileTag = settings.ProjectileTag;
            ShootingDelay = settings.ShootingDelay;
            MovementDelay = settings.MovementDelay;
            BulletDirection = settings.BulletDirection;
            MovementSmoothness = settings.MovementSmoothness;
            ProjectileIgnoreTags = settings.ProjectileIgnoreTags;
            VulnerableObjectsTags = settings.VulnerableObjectsTags;

            HalfVerticalSize = View.VerticalSize / 2;
            HalfHorizontalSize = View.HorizontalSize / 2;
        }
    }
}