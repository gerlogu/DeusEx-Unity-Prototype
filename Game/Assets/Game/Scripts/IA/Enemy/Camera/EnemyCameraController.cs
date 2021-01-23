using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script controlador de la cámara.
/// Representa la máquina de estados que
/// conforma la IA y gestiona el cambio
/// de estados según las condiciones en
/// las que se encuentre en el momento.
/// </summary>

public class EnemyCameraController : EnemyController
{
    public Transform cameraBase;
    public Transform cameraPivot;
    public EventTrigger cameraArea;
    public float currentRotation;
    public bool inverseRotation = true;

    [HideInInspector] public Quaternion originalCameraBaseRotation;

    protected override void Start()
    {
        base.Start();
        originalCameraBaseRotation = cameraBase.localRotation;
        stateMachine.SetInitialState(new CameraLookOutState(this, stateMachine));
    }

    public override void DetectPlayer()
    {
        if (!playerDetected)
        {
            playerDetected = true;
        }
            
    }

    public override void UndetectPlayer()
    {
        if (playerDetected)
        {
            playerDetected = false;
        }
    }
}
