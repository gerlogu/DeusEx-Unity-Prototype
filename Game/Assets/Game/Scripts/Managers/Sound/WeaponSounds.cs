using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weaponry.Sounds
{
    public class WeaponSounds : MonoBehaviour
    {
        public AudioClip shootSound;
        public float shootSoundVolume;

        public AudioClip reloadSound;
        public float reloadSoundVolume;

        public enum SoundType
        {
            SHOOT = 0,
            RELOAD = 1
        };

        public void PlaySFX(AudioSource audioSource, int soundType)
        {
            WeaponSounds.SoundType type = (WeaponSounds.SoundType)soundType;

            switch (type)
            {
                case WeaponSounds.SoundType.SHOOT:
                    audioSource.PlayOneShot(shootSound, shootSoundVolume);
                    break;
                case WeaponSounds.SoundType.RELOAD:
                    audioSource.PlayOneShot(reloadSound, reloadSoundVolume);
                    break;
                default:
                    Debug.LogWarning("SOUND NOT SUPPORTED.");
                    break;
            }
        }
    }
}
