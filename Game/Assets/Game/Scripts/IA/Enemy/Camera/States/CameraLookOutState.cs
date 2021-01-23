using UnityEngine;

/// <summary>
/// Clase que representa el estado de "vigilancia" 
/// de la cámara. Es el estado por defecto
/// en el que se encuentra desde el comienzo de la partida
/// </summary>

public class CameraLookOutState : State
{
    #region Variables
    private readonly EnemyCameraController _enemyController;
    private int _patrollingIndex;
    
    
    private float _timeToChangeDirection;
    #endregion

    #region Methods
    public CameraLookOutState(EnemyCameraController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;
        _enemyController.cameraBase.localRotation = _enemyController.originalCameraBaseRotation;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if(_timeToChangeDirection <= 0)
        {
            if (!_enemyController.inverseRotation)
            {
                _enemyController.currentRotation += 5 * Time.deltaTime;

                _enemyController.cameraPivot.localRotation = Quaternion.Euler(0, _enemyController.currentRotation, 0); 
                if (_enemyController.currentRotation >= 40)
                {
                    _enemyController.inverseRotation = true;
                    _timeToChangeDirection = 4;
                }
            }
            else
            {
                _enemyController.currentRotation -= 5 * Time.deltaTime;
                _enemyController.cameraPivot.localRotation = Quaternion.Euler(0, _enemyController.currentRotation, 0); // = Quaternion.Euler(Vector3.Lerp(_enemyController.cameraPivot.localRotation.eulerAngles, new Vector3(0, -45, 0), 1f * Time.deltaTime));
                if (_enemyController.currentRotation <= -40)
                {
                    _enemyController.inverseRotation = false;
                    _timeToChangeDirection = 4;
                }
            }
        }
        else
        {
            _timeToChangeDirection -= Time.deltaTime;
        }
        
        

        if (_enemyController.playerDetected && _enemyController.cameraArea.activated)
           stateMachine.SetState(new CameraPersecutionState(_enemyController, stateMachine));
        else if(!_enemyController.cameraArea.activated)
            stateMachine.SetState(new CameraShutDownState(_enemyController, stateMachine));
    }
    #endregion
}
