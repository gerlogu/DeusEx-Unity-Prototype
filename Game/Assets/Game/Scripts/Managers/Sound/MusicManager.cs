using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [HideInInspector] public float volume = 0;
    [HideInInspector] public float musicTransitionSpeed = 0;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = Mathf.Lerp(audioSource.volume, volume, musicTransitionSpeed * Time.deltaTime);
    }
}
