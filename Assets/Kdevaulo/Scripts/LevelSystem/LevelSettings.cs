using Kdevaulo.SpaceInvaders.EnemiesBehaviour;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders.LevelSystem
{
    [CreateAssetMenu(fileName = nameof(LevelSettings), menuName = nameof(LevelSystem) + "/" + nameof(LevelSettings))]
    public sealed class LevelSettings : ScriptableObject
    {
        [field: Header("Values")]
        [field: Min(0.00001f)]
        [field: SerializeField] public float VerticalStep { get; private set; }
        [field: SerializeField] public float EnemyShootDelay { get; private set; }
        [field: SerializeField] public Vector2 EnemiesBulletDirection { get; private set; }
        [field: SerializeField] public Vector2 EnemiesMoveDelayBounds { get; private set; }

        [field: SerializeField] public AnimationCurve EnemyMovementPattern { get; private set; }

        [field: SerializeField] public int ColumnsCount { get; private set; }
        [field: SerializeField] public EnemySettings[] EnemiesSettings { get; private set; }
    }
}