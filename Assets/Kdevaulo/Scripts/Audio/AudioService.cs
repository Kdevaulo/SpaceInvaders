using System.Collections.Generic;

using FMOD.Studio;

using FMODUnity;

using UnityEngine;

using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Kdevaulo.SpaceInvaders.Audio
{
    public static class AudioService
    {
        public static void PlayOneShot(EventReference sound, Vector3 worldPosition)
        {
            RuntimeManager.PlayOneShot(sound, worldPosition);
        }

        public static EventInstance Play(EventReference eventReference)
        {
            var eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstance.start();

            _soundEvents.Add(eventInstance);

            return eventInstance;
        }

        public static void StopEvent(EventInstance eventInstance, bool immediately = false)
        {
            var stopMode = immediately ? STOP_MODE.IMMEDIATE : STOP_MODE.ALLOWFADEOUT;
            eventInstance.stop(stopMode);
        }

        public static void ClearSoundEvents()
        {
            foreach (var sound in _soundEvents)
            {
                sound.stop(STOP_MODE.IMMEDIATE);
                sound.release();
            }
        }

        private static List<EventInstance> _soundEvents = new List<EventInstance>();
    }
}