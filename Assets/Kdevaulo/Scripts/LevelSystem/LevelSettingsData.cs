using Kdevaulo.SpaceInvaders.EnemiesBehaviour;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders.LevelngSystem
{
    [CreateAssetMenu(fileName = nameof(LevelSettingsData),
        menuName = nameof(LevelngSystem) + "/" + nameof(LevelSettingsData))]
    public sealed class LevelSettingsData : ScriptableObject
    {
        [field: Header("Values")]
        [field: SerializeField] public float EnemyVerticalStep { get; private set; }
        [field: Min(0.00001f)]
        [field: SerializeField] public float EnemyShootDelay { get; private set; }

        [field: SerializeField] public int EnemiesColumnsCount { get; private set; }

        [field: SerializeField] public Vector2 EnemiesBulletDirection { get; private set; }
        [field: SerializeField] public Vector2 EnemiesMoveDelayBounds { get; private set; }

        [field: SerializeField] public AnimationCurve EnemyMovementSpeedPattern { get; private set; }

        [field: SerializeField] public EnemySettings[] EnemiesSettings { get; private set; }
    }
}