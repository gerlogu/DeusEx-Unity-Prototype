using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShutDownState : State
{
    #region Variables
    private readonly EnemyCameraController _enemyController;
    private int _patrollingIndex;
    #endregion

    #region Methods
    public CameraShutDownState(EnemyCameraController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;
        // _enemyController.cameraPivot.localRotation = Quaternion.identity;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (_enemyController.cameraArea.activated)
            stateMachine.SetState(new CameraLookOutState(_enemyController, stateMachine));
    }
    #endregion
}
