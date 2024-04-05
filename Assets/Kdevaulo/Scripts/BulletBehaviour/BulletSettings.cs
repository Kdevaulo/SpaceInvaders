using UnityEngine;

namespace Kdevaulo.SpaceInvaders.BulletBehaviour
{
    [CreateAssetMenu(fileName = nameof(BulletSettings),
        menuName = nameof(BulletBehaviour) + "/" + nameof(BulletSettings))]
    public sealed class BulletSettings : ScriptableObject
    {
        [field: SerializeField] public float ProjectileMoveDelay { get; private set; }
    }
}