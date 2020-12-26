using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundList : MonoBehaviour
{
    [Header("Player")]
    public AudioClip[] stepsSound;
    [Range(0,1)] public float stepsSoundVolume = 1;

    public AudioClip jumpSound;
    [Range(0, 1)] public float jumpSoundVolume = 1;

    public AudioClip jumpImpactSound;
    [Range(0, 1)] public float jumpImpactSoundVolume = 1;

    public AudioClip dashSound;
    [Range(0, 1)] public float dashSoundVolume = 1;

    public AudioClip hookSound;
    [Range(0, 1)] public float hookSoundVolume = 1;

    public AudioClip wallGrabSound;
    [Range(0, 1)] public float wallGrabSoundVolume = 1;

    [Header("Weapons>Pistol")]
    [Header("Weapons")]
    public AudioClip pistolShootSound;
    [Range(0, 1)] public float pistolShootSoundVolume = 1;

    public AudioClip pistolReloadSound;
    [Range(0, 1)] public float pistolReloadSoundVolume = 1;

    [Header("Weapons>Rifle")]
    public AudioClip rifleShootSound;
    [Range(0, 1)] public float rifleShootSoundVolume = 1;

    public AudioClip rifleReloadSound;
    [Range(0, 1)] public float rifleReloadSoundVolume = 1;


    [Header("Enemy")]
    public AudioClip enemyReceiveDamageSound;
    [Range(0, 1)] public float enemyReceiveDamageSoundVolume = 1;

    [Header("Game Elements")]
    public AudioClip barrelExplosionSound;
    [Range(0, 1)] public float barrelExplosionSoundVolume = 1;
}
