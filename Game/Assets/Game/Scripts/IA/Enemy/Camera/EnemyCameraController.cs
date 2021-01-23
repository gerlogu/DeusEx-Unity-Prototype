using UnityEngine;

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
