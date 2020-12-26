using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    private CameraFOVController cam;
    [SerializeField] private Slider fielOfView;
    [SerializeField] private float FOV_90  = 58.71551f;
    [SerializeField] private float FOV_95  = 63.08829f;
    [SerializeField] private float FOV_100 = 67.67275f;
    [SerializeField] private float FOV_105 = 72.48763f;
    [SerializeField] private float FOV_110 = 77.55214f;
    [SerializeField] private TextMeshProUGUI fovText;

    private int fov = 0;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFOVController>();
        fielOfView.value = cam.fieldOfView;
    }

   

    // Update is called once per frame
    void Update()
    {
        
        if (fielOfView.value > 77.5f)
        {
            cam.fieldOfView = FOV_110;
            fov = 110;
        }
        else if (fielOfView.value > 72.4f)
        {
            cam.fieldOfView = FOV_105;
            fov = 105;
        }
        else if (fielOfView.value > 67.6f)
        {
            cam.fieldOfView = FOV_100;
            fov = 100;
            
        }
        else if (fielOfView.value > 63.0)
        {
            cam.fieldOfView = FOV_95;
            fov = 95;
        }
        else if (fielOfView.value > 58.7f)
        {
            
            cam.fieldOfView = FOV_90;
            fov = 90;
        }
        else
        {
            print("FOV: under 90");
        }

        fovText.text = fov.ToString();
    }
}
