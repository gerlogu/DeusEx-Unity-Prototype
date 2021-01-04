using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersecutionState : State
{
    #region Variables
    private readonly SoldierController _enemyController;
    private float lostLocationTimer;
    #endregion

    public PersecutionState(SoldierController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;
        lostLocationTimer = 5;
        _enemyController.mesh.material = _enemyController.persecutionMaterial;

        _enemyController.stateTextAnimator.SetBool("Alert", false);
        _enemyController.stateTextAnimator.SetBool("Persecution", true);
        _enemyController.stateTextAnimator.SetBool("None", false);
        _enemyController.stateText.text = "!";
        _enemyController.playerDetected = true;
        _enemyController.Agent.speed = _enemyController.persecutionSpeed;
        _enemyController.Agent.stoppingDistance = 10f;
        // if (_enemyController.Agent.enabled)
        // _enemyController.Agent.isStopped = true;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (Physics.CheckSphere(_enemyController.transform.position, _enemyController.lookingForPlayerArea, 1 << 10))
        {
           //  Debug.Log("Player in Area");
            if(_enemyController.CheckPlayerVision(_enemyController.player.transform.position, 10))
            {
                _enemyController.Agent.SetDestination(_enemyController.player.position);
                lostLocationTimer = 5;
                Quaternion currentRot = Quaternion.LookRotation(_enemyController.player.position - _enemyController.transform.position);
                _enemyController.transform.rotation = Quaternion.Lerp(_enemyController.transform.rotation, currentRot, 8 * Time.deltaTime);
                Debug.Log("Persecution");
            }
            
        }

        if (lostLocationTimer <= 0)
        {
            stateMachine.SetState(new PatrollingState(_enemyController, stateMachine, _enemyController.currentPatrollingIndex));
        }
        else
        {
            lostLocationTimer -= Time.deltaTime;
        }

       
    }
}
