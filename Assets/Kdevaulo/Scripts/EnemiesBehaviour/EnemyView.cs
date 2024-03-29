using UnityEngine;

namespace Kdevaulo.SpaceInvaders.EnemiesBehaviour
{
    [AddComponentMenu(nameof(EnemyView) + " in " + nameof(EnemiesBehaviour))]
    public sealed class EnemyView : MonoBehaviour
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