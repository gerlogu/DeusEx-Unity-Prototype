using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameElementsSound
{
    BARREL_EXPLOSION = 0,
};

public class SoundManager : MonoBehaviour
{

    private SoundList soundList;
    private MusicList musicList;

    #region Player Sounds
    public PlayerSounds player;

    public struct PlayerSounds
    {
        public AudioClip[] stepsSound;
        public float stepsSoundVolume;

        public AudioClip jumpSound;
        public float jumpSoundVolume;

        public AudioClip jumpImpactSound;
        public float jumpImpactSoundVolume;

        public AudioClip dashSound;
        public float dashSoundVolume;

        public AudioClip hookSound;
        public float hookSoundVolume;

        public AudioClip wallGrabSound;
        public float wallGrabSoundVolume;

        public enum SoundType {
            STEPS = 0,
            JUMP = 1,
            JUMP_IMPACT = 2,
            DASH = 3,
            HOOK = 4,
            WALL_GRAB = 5
        };

        public void InitSounds(SoundList soundList)
        {
            stepsSound = soundList.stepsSound;
            stepsSoundVolume = soundList.stepsSoundVolume;

            jumpSound = soundList.jumpSound;
            jumpSoundVolume = soundList.jumpSoundVolume;

            jumpImpactSound = soundList.jumpImpactSound;
            jumpImpactSoundVolume = soundList.jumpImpactSoundVolume;

            dashSound = soundList.dashSound;
            dashSoundVolume = soundList.dashSoundVolume;

            hookSound = soundList.hookSound;
            hookSoundVolume = soundList.hookSoundVolume;

            wallGrabSound = soundList.wallGrabSound;
            wallGrabSoundVolume = soundList.wallGrabSoundVolume;
    }

        public void PlaySFX(AudioSource audioSource, int soundType)
        {
            PlayerSounds.SoundType type = (PlayerSounds.SoundType)soundType;

            switch (type)
            {
                case PlayerSounds.SoundType.STEPS:
                    int random = Random.Range(0, stepsSound.Length);
                    audioSource.PlayOneShot(stepsSound[random], stepsSoundVolume);
                    break;
                case PlayerSounds.SoundType.JUMP:
                    audioSource.PlayOneShot(jumpSound, jumpSoundVolume);
                    break;
                case PlayerSounds.SoundType.JUMP_IMPACT:
                    audioSource.PlayOneShot(jumpImpactSound, jumpImpactSoundVolume);
                    break;
                case PlayerSounds.SoundType.DASH:
                    audioSource.PlayOneShot(dashSound, dashSoundVolume);
                    break;
                case PlayerSounds.SoundType.HOOK:
                    audioSource.PlayOneShot(hookSound, hookSoundVolume);
                    break;
                case PlayerSounds.SoundType.WALL_GRAB:
                    audioSource.PlayOneShot(wallGrabSound, wallGrabSoundVolume);
                    break;
                default:
                    Debug.LogWarning("SOUND NOT SUPPORTED.");
                    break;
            }
        }
    }


    #endregion

    #region Weapon Sounds
    public WeaponSounds weapons;

    public struct WeaponSounds
    {
        public Weaponry.Sounds.WeaponSounds pistol;

        public void InitSounds(SoundList soundList)
        {
            pistol = new Weaponry.Sounds.WeaponSounds();
            pistol.shootSound = soundList.pistolShootSound;
            pistol.shootSoundVolume = soundList.pistolShootSoundVolume;
            pistol.reloadSound = soundList.pistolReloadSound;
            pistol.reloadSoundVolume = soundList.pistolReloadSoundVolume;
        }
    }
    #endregion

    #region Enemy Sounds
    public EnemySounds enemy;

    public struct EnemySounds
    {
        public AudioClip receiveDamageSound;
        public float receiveDamageSoundVolume;

        public enum SoundType
        {
            RECEIVE_DAMAGE = 0,
        };

        public void InitSounds(SoundList soundList)
        {
            receiveDamageSound = soundList.enemyReceiveDamageSound;
            receiveDamageSoundVolume = soundList.enemyReceiveDamageSoundVolume;
        }

        public void PlaySFX(AudioSource audioSource, int soundType)
        {
            SoundType type = (SoundType)soundType;

            switch (type)
            {
                case SoundType.RECEIVE_DAMAGE:
                    audioSource.PlayOneShot(receiveDamageSound, receiveDamageSoundVolume);
                    break;
                default:
                    Debug.LogWarning("SOUND NOT SUPPORTED.");
                    break;
            }
        }
    }
    #endregion

    #region Battle Music
    public BattleMusic battleMusic;

    public struct BattleMusic
    {
        public List<AudioClip> themes;
        public List<float> volumes;

        public enum SoundType
        {
            RECEIVE_DAMAGE = 0,
        };

        public void InitThemes(MusicList musicList)
        {
            themes = new List<AudioClip>();
            volumes = new List<float>();

            #region Themes
            themes.Add(musicList.actionMusic0);
            themes.Add(musicList.actionMusic1);
            themes.Add(musicList.actionMusic2);
            themes.Add(musicList.actionMusic3);
            themes.Add(musicList.actionMusic4);
            themes.Add(musicList.actionMusic5);
            themes.Add(musicList.actionMusic6);
            themes.Add(musicList.actionMusic7);
            themes.Add(musicList.actionMusic8);
            themes.Add(musicList.actionMusic9);
            themes.Add(musicList.actionMusic10);
            themes.Add(musicList.actionMusic11);
            themes.Add(musicList.actionMusic12);
            themes.Add(musicList.actionMusic13);
            themes.Add(musicList.actionMusic14);
            themes.Add(musicList.actionMusic15);
            themes.Add(musicList.actionMusic16);
            themes.Add(musicList.actionMusic17);
            themes.Add(musicList.actionMusic18);
            themes.Add(musicList.actionMusic19);
            themes.Add(musicList.actionMusic20);
            themes.Add(musicList.actionMusic21);
            themes.Add(musicList.actionMusic22);
            themes.Add(musicList.actionMusic23);
            themes.Add(musicList.actionMusic24);
            themes.Add(musicList.actionMusic25);
            themes.Add(musicList.actionMusic26);
            themes.Add(musicList.actionMusic27);
            themes.Add(musicList.actionMusic28);
            themes.Add(musicList.actionMusic29);
            themes.Add(musicList.actionMusic30);
            themes.Add(musicList.actionMusic31);
            themes.Add(musicList.actionMusic32);
            themes.Add(musicList.actionMusic33);
            themes.Add(musicList.actionMusic34);
            themes.Add(musicList.actionMusic35);
            themes.Add(musicList.actionMusic36);
            themes.Add(musicList.actionMusic37);
            themes.Add(musicList.actionMusic38);
            themes.Add(musicList.actionMusic39);
            themes.Add(musicList.actionMusic40);
            themes.Add(musicList.actionMusic41);
            themes.Add(musicList.actionMusic42);
            themes.Add(musicList.actionMusic43);
            themes.Add(musicList.actionMusic44);
            themes.Add(musicList.actionMusic45);
            themes.Add(musicList.actionMusic46);
            themes.Add(musicList.actionMusic47);
            #endregion

            #region Theme Volumes
            volumes.Add(musicList.actionMusic0Volume);
            volumes.Add(musicList.actionMusic1Volume);
            volumes.Add(musicList.actionMusic2Volume);
            volumes.Add(musicList.actionMusic3Volume);
            volumes.Add(musicList.actionMusic4Volume);
            volumes.Add(musicList.actionMusic5Volume);
            volumes.Add(musicList.actionMusic6Volume);
            volumes.Add(musicList.actionMusic7Volume);
            volumes.Add(musicList.actionMusic8Volume);
            volumes.Add(musicList.actionMusic9Volume);
            volumes.Add(musicList.actionMusic10Volume);
            volumes.Add(musicList.actionMusic11Volume);
            volumes.Add(musicList.actionMusic12Volume);
            volumes.Add(musicList.actionMusic13Volume);
            volumes.Add(musicList.actionMusic14Volume);
            volumes.Add(musicList.actionMusic15Volume);
            volumes.Add(musicList.actionMusic16Volume);
            volumes.Add(musicList.actionMusic17Volume);
            volumes.Add(musicList.actionMusic18Volume);
            volumes.Add(musicList.actionMusic19Volume);
            volumes.Add(musicList.actionMusic20Volume);
            volumes.Add(musicList.actionMusic21Volume);
            volumes.Add(musicList.actionMusic22Volume);
            volumes.Add(musicList.actionMusic23Volume);
            volumes.Add(musicList.actionMusic24Volume);
            volumes.Add(musicList.actionMusic25Volume);
            volumes.Add(musicList.actionMusic26Volume);
            volumes.Add(musicList.actionMusic27Volume);
            volumes.Add(musicList.actionMusic28Volume);
            volumes.Add(musicList.actionMusic29Volume);
            volumes.Add(musicList.actionMusic30Volume);
            volumes.Add(musicList.actionMusic31Volume);
            volumes.Add(musicList.actionMusic32Volume);
            volumes.Add(musicList.actionMusic33Volume);
            volumes.Add(musicList.actionMusic34Volume);
            volumes.Add(musicList.actionMusic35Volume);
            volumes.Add(musicList.actionMusic36Volume);
            volumes.Add(musicList.actionMusic37Volume);
            volumes.Add(musicList.actionMusic38Volume);
            volumes.Add(musicList.actionMusic39Volume);
            volumes.Add(musicList.actionMusic40Volume);
            volumes.Add(musicList.actionMusic41Volume);
            volumes.Add(musicList.actionMusic42Volume);
            volumes.Add(musicList.actionMusic43Volume);
            volumes.Add(musicList.actionMusic44Volume);
            volumes.Add(musicList.actionMusic45Volume);
            volumes.Add(musicList.actionMusic46Volume);
            volumes.Add(musicList.actionMusic47Volume);
            #endregion
        }

        public void PlayWithDefaultVolume(AudioSource audioSource, int index)
        {
            if (index > -1 && index < themes.Count)
                audioSource.PlayOneShot(themes[index], volumes[index]);
            else
                Debug.LogError("NO MUSIC THEME CONTAINED IN PLAYLIST.");
        }

        public void Play(AudioSource audioSource, int index, out float objectiveVolume)
        {

            if (index > -1 && index < themes.Count)
            {
                audioSource.clip = themes[index];
                audioSource.Play();
                objectiveVolume = volumes[index];
            }
            else
            {
                Debug.LogError("NO MUSIC THEME CONTAINED IN PLAYLIST.");
                objectiveVolume = 0;
            }
                
        }

        public void Stop(out float objectiveVolume)
        {
            objectiveVolume = 0;
        }
    }
    #endregion

    #region Game Elements Sounds
    public GameElementsSounds gameElements;

    public struct GameElementsSounds
    {
        public AudioClip barrelExplosionSound;
        public float barrelExplosionSoundVolume;

        

        public void InitSounds(SoundList soundList)
        {
            barrelExplosionSound = soundList.barrelExplosionSound;
            barrelExplosionSoundVolume = soundList.barrelExplosionSoundVolume;
        }

        public void PlaySFX(AudioSource audioSource, int soundType)
        {
            GameElementsSound type = (GameElementsSound)soundType;

            switch (type)
            {
                case GameElementsSound.BARREL_EXPLOSION:
                    audioSource.PlayOneShot(barrelExplosionSound, barrelExplosionSoundVolume);
                    break;
                default:
                    Debug.LogWarning("SOUND NOT SUPPORTED.");
                    break;
            }
        }

        public void PlaySFX(AudioSource audioSource, GameElementsSound gameElementSound)
        {
            switch (gameElementSound)
            {
                case GameElementsSound.BARREL_EXPLOSION:
                    audioSource.PlayOneShot(barrelExplosionSound, barrelExplosionSoundVolume);
                    break;
                default:
                    Debug.LogWarning("SOUND NOT SUPPORTED.");
                    break;
            }
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        soundList = GetComponent<SoundList>();
        musicList = GetComponent<MusicList>();
        player.InitSounds(soundList);
        weapons.InitSounds(soundList);
        enemy.InitSounds(soundList);
        gameElements.InitSounds(soundList);
        battleMusic.InitThemes(musicList);
    }
}
