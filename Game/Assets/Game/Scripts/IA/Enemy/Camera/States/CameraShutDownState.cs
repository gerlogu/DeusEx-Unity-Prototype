using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que representa el estado de "muerte"
/// de la cámara. Se llega a este mismo a través
/// de la eliminación del operario que maneja
/// la cámara.
/// </summary>

public class CameraShutDownState : State
{
    #region Variables
    private readonly EnemyCameraController _enemyController;
    #endregion

    #region Methods
    public CameraShutDownState(EnemyCameraController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (_enemyController.cameraArea.activated)
            stateMachine.SetState(new CameraLookOutState(_enemyController, stateMachine));
    }
    #endregion
}
