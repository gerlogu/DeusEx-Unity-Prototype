using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunManager : MonoBehaviour
{

    [SerializeField] GameObject sun;

    private float sunMovementVelocity = 2f;
    [HideInInspector] public Quaternion currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        currentRotation = sun.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        sun.transform.rotation = Quaternion.Lerp(sun.transform.rotation, currentRotation, sunMovementVelocity * Time.deltaTime);
    }
}
