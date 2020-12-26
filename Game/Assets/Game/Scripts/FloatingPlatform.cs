using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;
    [SerializeField] private float movementSpeed;

    [SerializeField] private bool isGoingUp;
    bool isGoingDown;

    // Start is called before the first frame update
    void Start()
    {
        maxHeight = maxHeight + transform.position.y;
        minHeight = minHeight - transform.position.y;

        if (!isGoingUp)
        {
            isGoingDown = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoingUp)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + movementSpeed * Time.deltaTime, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - movementSpeed * Time.deltaTime, transform.position.z);
        }
        
        if(transform.position.y >= maxHeight - 0.05)
        {
            isGoingUp = false;
            isGoingDown = true;
        }

        if (transform.position.y <= minHeight + 0.05)
        {
            isGoingUp = true;
            isGoingDown = false;
        }
    }
}
