using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : MonoBehaviour
{
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovementOld>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
    }
}
