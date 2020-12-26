using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFOVFixer : MonoBehaviour
{
    private CameraFOVController cam;

    public Vector3 FOV_90;
    public Vector3 FOV_95;
    public Vector3 FOV_100;
    public Vector3 FOV_105;
    public Vector3 FOV_110;

    [SerializeField] private float lerpSmooth = 6;

    private GameManager gameManager;

    public Vector3 localPosition;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFOVController>();
        gameManager = FindObjectOfType<GameManager>();
        lerpSmooth = cam.lerpSmooth;
        SetFOV();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager)
        {
            if (gameManager.CheckPlayerState(1))
            {
                SetFOV();
            }
        }
        else
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, localPosition.z), Time.unscaledDeltaTime * lerpSmooth);
    }

    

    private void SetFOV()
    {
        if (cam.fieldOfView > 77.5f)
        {
            localPosition = FOV_110;
        }
        else if (cam.fieldOfView > 72.4f)
        {
            localPosition = FOV_105;
        }
        else if (cam.fieldOfView > 67.6f)
        {
            localPosition = FOV_100;
        }
        else if (cam.fieldOfView > 63.0f)
        {
            localPosition = FOV_95;
        }
        else if (cam.fieldOfView > 58.7f)
        {
            localPosition = FOV_90;
        }
    }
}
