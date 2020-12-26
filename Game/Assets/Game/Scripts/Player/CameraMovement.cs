using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivityX = 100f;
    public float sensitivityY = 100f;

    public Transform playerBody;

    public bool leftWall;
    public bool rightWall;
    public float smoothRotation;

    public GameObject cam;

    public GameObject pelvis;
    Vector3 rotation;

    float xRotation = 0f;

    public bool canMove = true;

    private void Start()
    {
        //if (canMove)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;
        //}
        //else
        //{
        //    Cursor.lockState = CursorLockMode.None;
        //    Cursor.visible = true;
        //}
        rotation = this.transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            MouseMovement();
    }

    private void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerBody.Rotate(Vector3.up * mouseX);
        // playerBody.GetComponent<PlayerController>().rb.rotation = new Quaternion(playerBody.GetComponent<PlayerController>().rb.rotation.x, (Vector3.up * mouseX).y, playerBody.GetComponent<PlayerController>().rb.rotation.z, 0);


        

        //rotation = this.transform.localRotation.eulerAngles;

        if (leftWall)
        {
            rotation = Vector3.Lerp(rotation, new Vector3(cam.transform.localRotation.eulerAngles.x, cam.transform.localRotation.eulerAngles.y, -8), smoothRotation * Time.deltaTime);
           // this.transform.localRotation = Quaternion.Euler(rotation);

        }
        else if (rightWall)
        {
            rotation = Vector3.Lerp(rotation, new Vector3(cam.transform.localRotation.eulerAngles.x, cam.transform.localRotation.eulerAngles.y, 8), smoothRotation * Time.deltaTime);
           // this.transform.localRotation = Quaternion.Euler(rotation);
        }
        else
        {
            rotation = Vector3.Lerp(rotation, new Vector3(this.transform.localRotation.eulerAngles.x, this.transform.localRotation.eulerAngles.y, 0), smoothRotation / 1.5f * Time.deltaTime);
            // this.transform.localRotation = Quaternion.Euler(rotation);
        }

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        pelvis.transform.localRotation = Quaternion.Euler(0f, 0f, rotation.z);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
