using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOVController : MonoBehaviour
{
    [HideInInspector] public float fieldOfView;
    public float lerpSmooth = 6;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        fieldOfView = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fieldOfView, Time.unscaledDeltaTime * lerpSmooth);
    }
}
