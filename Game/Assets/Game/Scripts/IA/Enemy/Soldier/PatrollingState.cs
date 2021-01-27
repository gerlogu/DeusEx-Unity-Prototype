using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que representa el estado de "patrulla" 
/// del soldado. Es su estado por defecto, 
/// realiza un recorrido entre los distintos 
/// puntos establecidos en Unity.
/// </summary>

public class PatrollingState : State
{
    #region Variables
    private readonly SoldierController _enemyController;
    private int _patrollingIndex;
    #endregion

    #region Methods
    public PatrollingState(SoldierController enemyController, MyStateMachine stateMachine, int patrollingIndex) : base(stateMachine)
    {
        _patrollingIndex = patrollingIndex;
        _enemyController = enemyController;
        _enemyController.mesh.material = _enemyController.patrollingMaterial;
        _enemyController.stateTextAnimator.SetBool("Alert", false);
        _enemyController.stateTextAnimator.SetBool("Persecution", false);
        _enemyController.stateTextAnimator.SetBool("None", true);
        _enemyController.Agent.speed = _enemyController.normalSpeed;
        _enemyController.playerDetected = false;
        _enemyController.Agent.stoppingDistance = 1f;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        _enemyController.Agent.SetDestination(_enemyController.points[_patrollingIndex].position);

        Collider[] hitColliders = Physics.OverlapSphere(_enemyController.transform.position, 2.6f, 1 << 25);

        if (hitColliders.Length > 0)
        {
            foreach (Collider col in hitColliders)
            {
                if (col.transform == _enemyController.points[_patrollingIndex])
                {
                    stateMachine.SetState(new RestingState(_enemyController, stateMachine));
                }
            }
        }
    }
    #endregion
}
