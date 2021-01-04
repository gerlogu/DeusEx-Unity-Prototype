using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingState : State
{
    #region Variables
    private readonly SoldierController _enemyController;
    private float _timeToRest;
    #endregion

    public RestingState(SoldierController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;
        _timeToRest = Random.Range(4, 8);
        _enemyController.playerDetected = false;
        if (_enemyController.currentPatrollingIndex == _enemyController.points.Length - 1)
        {
            _enemyController.currentPatrollingIndex = 0;
        }
        else
        {
            _enemyController.currentPatrollingIndex++;
        }
        _enemyController.mesh.material = _enemyController.restingMaterial;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if(_timeToRest <= 0)
        {
            stateMachine.SetState(new PatrollingState(_enemyController, stateMachine, _enemyController.currentPatrollingIndex));
        }
        else
        {
            _timeToRest -= Time.deltaTime;
        }
    }
}
