using UnityEngine;

namespace Kdevaulo.SpaceInvaders.Audio
{
    [AddComponentMenu(nameof(BackgroundMusicPlayer) + " in " + nameof(Audio))]
    public class BackgroundMusicPlayer : MonoBehaviour
    {
        private void Start()
        {
            AudioService.Play(AudioContainer.Instance.BackgroundMusic);
        }
    }
}