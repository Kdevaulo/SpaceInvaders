using UnityEngine;

namespace Kdevaulo.SpaceInvaders.ProjectileBehaviour
{
    public sealed class ProjectileModel : BaseModel<MovingItemView>
    {
        public readonly Vector2 Direction;

        public ProjectileModel(MovingItemView view, Vector2 direction, Vector2 startPosition) : base(view)
        {
            Direction = direction;
            Position = startPosition;

            var bounds = view.Collider.bounds;
            HalfVerticalSize = bounds.size.y / 2;
            HalfHorizontalSize = bounds.size.x / 2;
        }
    }
}