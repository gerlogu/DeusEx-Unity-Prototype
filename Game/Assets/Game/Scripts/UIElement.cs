using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    Transform mainCameraTransform;
    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (Camera.main)
            mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mainCameraTransform && Camera.main)
        {
            mainCameraTransform = Camera.main.transform;
        }

        if (Camera.main && mainCameraTransform == null)
            mainCameraTransform = Camera.main.transform;
        else
        {
            if (rectTransform)
            {
                rectTransform.LookAt(mainCameraTransform);
            }
            else
            {
                transform.LookAt(mainCameraTransform);
            }
        }

    }
}
