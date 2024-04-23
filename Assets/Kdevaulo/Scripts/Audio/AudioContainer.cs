using FMODUnity;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.SpaceInvaders.Audio
{
    [AddComponentMenu(nameof(AudioContainer) + " in " + nameof(Audio))]
    public class AudioContainer : MonoBehaviour
    {
        [field: Header("SFX")]
        [field: SerializeField] public EventReference Shoot { get; private set; }
        [field: SerializeField] public EventReference EnemySpawn { get; private set; }
        [field: SerializeField] public EventReference EnemyMoveDown { get; private set; }
        [field: Header("Music")]
        [field: SerializeField] public EventReference BackgroundMusic { get; private set; }

        public static AudioContainer Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, $"Found more than one {nameof(AudioContainer)} in the scene.");

            Instance = this;
        }
    }
}