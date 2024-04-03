using UnityEngine;

namespace Kdevaulo.SpaceInvaders
{
    [AddComponentMenu(nameof(MovingItemView) + " in " + nameof(SpaceInvaders))]
    public class MovingItemView : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public Collider2D Collider { get; private set; }

        [SerializeField] private Transform _transform;

        public void SetPosition(Vector2 position)
        {
            _transform.position = position;
        }
    }
}