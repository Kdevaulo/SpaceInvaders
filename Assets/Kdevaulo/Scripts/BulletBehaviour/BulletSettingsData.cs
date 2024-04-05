using UnityEngine;

namespace Kdevaulo.SpaceInvaders.BulletBehaviour
{
    [CreateAssetMenu(fileName = nameof(BulletSettingsData),
        menuName = nameof(BulletBehaviour) + "/" + nameof(BulletSettingsData))]
    public sealed class BulletSettingsData : ScriptableObject
    {
        [field: SerializeField] public float ProjectileMoveDelay { get; private set; }
    }
}