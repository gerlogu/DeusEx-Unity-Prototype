using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : State
{
    #region Variables
    private readonly SoldierController _enemyController;
    private readonly Vector3 suspectPosition;
    private float _timeToStartPatrolling;
    #endregion

    public AlertState(SoldierController enemyController, MyStateMachine stateMachine, Vector3 position) : base(stateMachine)
    {
        _enemyController = enemyController;

        _timeToStartPatrolling = 4;

        suspectPosition = position;

        _enemyController.mesh.material = _enemyController.alertMaterial;

        _enemyController.Agent.stoppingDistance = 0.5f;

        _enemyController.Agent.SetDestination(suspectPosition);

        _enemyController.stateTextAnimator.SetBool("Alert", true);
        _enemyController.stateTextAnimator.SetBool("Persecution", false);
        _enemyController.stateTextAnimator.SetBool("None", false);
        _enemyController.stateText.text = "?";
        //if (_enemyController.Agent.enabled)
        //    _enemyController.Agent.isStopped = true;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if(_timeToStartPatrolling <= 0)
        {
            stateMachine.SetState(new PatrollingState(_enemyController, stateMachine, _enemyController.currentPatrollingIndex));
        }
        else
        {
            _timeToStartPatrolling -= Time.deltaTime;
        }
    }
}
