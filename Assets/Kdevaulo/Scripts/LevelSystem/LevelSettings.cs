using Kdevaulo.SpaceInvaders.EnemiesBehaviour;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders.LevelSystem
{
    [CreateAssetMenu(fileName = nameof(LevelSettings), menuName = nameof(LevelSystem) + "/" + nameof(LevelSettings))]
    public sealed class LevelSettings : ScriptableObject
    {
        [field: Header("Values")]
        [field: SerializeField] public float EnemiesStartSpeed { get; private set; }
        [field: SerializeField] public float EnemiesSpeedStep { get; private set; }

        [field: Header("References")]
        [field: SerializeField] public EnemySettings[] EnemiesSettings { get; private set; }
    }
}