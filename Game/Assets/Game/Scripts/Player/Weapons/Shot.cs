using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    bool canUpdateValue = true;

    private void Start()
    {
        //Physics.IgnoreLayerCollision(10, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (canUpdateValue)
        {
            print(other.gameObject.name);
            Destroy(this);
            canUpdateValue = false;
            
        }
        
    }
}
