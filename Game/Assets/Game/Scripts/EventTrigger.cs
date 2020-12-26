using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    BATTLE_MUSIC,
    BACKGROUND_MUSIC,
    STOP_MUSIC,
    START_BATTLE,
    DEATH,
    SUN_MODIFIER
}

public class EventTrigger : MonoBehaviour
{
    public EventType eventType;
    public int themeIndex;
    public float musicTransitionSpeed = 0.75f;
    public Vector3 sunRotation;

    private float objectiveVolume = 0;
    private MusicManager musicManager;
    private AudioSource audioSource;
    private bool activated = false;
    private Transform player;
    private LayerMask playerLayer;
    private Vector3 respawnPoint;

    private void Awake()
    {
        playerLayer = 10;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        musicManager = FindObjectOfType<MusicManager>();
        audioSource = musicManager.GetComponent<AudioSource>();
        if(!player)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource.volume = 0;
        respawnPoint = player.position;
    }

    private void Update()
    {
        //Debug.Log(playerLayer.value);
        //if(Physics.BoxCast(transform.position, transform.localScale, new Vector3(0,0,0), transform.rotation, playerLayer))
        //{
        //    Debug.Log("Player Detected");
        //}

        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        switch (eventType)
        {
            case EventType.BATTLE_MUSIC:
                if (!activated)
                {
                    if (other.CompareTag("Player"))
                    {
                        FindObjectOfType<SoundManager>().battleMusic.Play(audioSource, themeIndex - 1, out objectiveVolume);
                        musicManager.volume = objectiveVolume;
                        musicManager.musicTransitionSpeed = musicTransitionSpeed;
                        activated = true;
                    }
                    
                }
                break;
            case EventType.STOP_MUSIC:
                if (!activated)
                {
                    if (other.CompareTag("Player"))
                    {
                        FindObjectOfType<SoundManager>().battleMusic.Stop(out objectiveVolume);
                        musicManager.volume = objectiveVolume;
                        musicManager.musicTransitionSpeed = musicTransitionSpeed;
                        activated = true;
                    }
                    
                }
                break;
            case EventType.DEATH:
                if (other.CompareTag("Player"))
                {
                    //player.position = respawnPoint;
                    other.GetComponent<CharacterController>().enabled = false;
                    other.transform.position = respawnPoint;
                    print(other.name);
                    other.GetComponent<CharacterController>().enabled = true;
                }
                break;
            case EventType.SUN_MODIFIER:
                if (!activated)
                {
                    if (other.CompareTag("Player"))
                    {
                        FindObjectOfType<SunManager>().currentRotation = Quaternion.Euler(sunRotation);
                        activated = true;
                    }
                }
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if(eventType == EventType.BATTLE_MUSIC || eventType == EventType.BACKGROUND_MUSIC)
            Gizmos.color = new Color(0, 1, 0, 0.5f);
        else if (eventType == EventType.STOP_MUSIC)
            Gizmos.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.5f);
        else if(eventType == EventType.DEATH)
            Gizmos.color = new Color(1, 0, 0, 0.5f);

        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
