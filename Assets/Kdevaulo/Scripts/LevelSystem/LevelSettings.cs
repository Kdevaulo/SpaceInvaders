using Kdevaulo.SpaceInvaders.EnemiesBehaviour;
using Kdevaulo.SpaceInvaders.PlayerBehaviour;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders.LevelSystem
{
    [CreateAssetMenu(fileName = nameof(LevelSettings), menuName = nameof(LevelSystem) + "/" + nameof(LevelSettings))]
    public sealed class LevelSettings : ScriptableObject
    {
        [field: Header("Values")]
        [field: Min(0.00001f)]
        [field: SerializeField] public float EnemiesStartSpeed { get; private set; }

        [field: Min(0.00001f)]
        [field: SerializeField] public float EnemiesSpeedStep { get; private set; }
        [field: SerializeField] public float VerticalStep { get; private set; }
        [field: SerializeField] public PlayerSettings PlayerSettings { get; private set; }
        [field: SerializeField] public EnemySettings[] EnemiesSettings { get; private set; }
    }
}