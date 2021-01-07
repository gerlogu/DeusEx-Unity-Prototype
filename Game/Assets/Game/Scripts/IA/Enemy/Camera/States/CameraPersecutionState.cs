using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPersecutionState : State
{
    #region Variables
    private readonly EnemyCameraController _enemyController;
    #endregion

    #region Methods
    public CameraPersecutionState(EnemyCameraController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;
        _enemyController.cameraPivot.localRotation = Quaternion.identity;
        // _enemyController.mesh.material = _enemyController.patrollingMaterial;
        // _enemyController.stateTextAnimator.SetBool("Alert", false);
        // _enemyController.stateTextAnimator.SetBool("Persecution", false);
        // _enemyController.stateTextAnimator.SetBool("None", true);
        // _enemyController.Agent.speed = _enemyController.normalSpeed;
        // _enemyController.playerDetected = false;
        // _enemyController.Agent.stoppingDistance = 1f;
        // Debug.Log("Patrolling State");
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        
        if (_enemyController.CheckPlayerVision(_enemyController.player.transform.position, 10))
        {
            Quaternion currentRot = Quaternion.LookRotation(_enemyController.player.position - _enemyController.transform.position);
            _enemyController.cameraBase.localRotation = Quaternion.Lerp(_enemyController.cameraBase.localRotation, currentRot, 8 * Time.deltaTime);
        }
        else
        {
            _enemyController.playerDetected = false;
            stateMachine.SetState(new CameraLookOutState(_enemyController, stateMachine));
        }

        if(!_enemyController.cameraArea.activated)
        {
            stateMachine.SetState(new CameraShutDownState(_enemyController, stateMachine));
        }
    }
    #endregion
}
