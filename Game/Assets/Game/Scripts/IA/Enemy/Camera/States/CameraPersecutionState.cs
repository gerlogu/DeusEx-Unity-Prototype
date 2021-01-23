using System.Collections.Generic;
using UnityEngine;

public class CameraPersecutionState : State
{
    #region Variables
    private readonly EnemyCameraController _enemyController;
    private float _askForHelpTime;
    private List<EnemyController> _allies;
    private bool _askedForHelp = false;
    #endregion

    #region Methods
    public CameraPersecutionState(EnemyCameraController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;
        _enemyController.cameraPivot.localRotation = Quaternion.identity;

        _allies = new List<EnemyController>();
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(_enemyController.transform.position, 40, _enemyController.player.transform.up, 1 << 16);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.GetComponent<EnemyController>())
            {
                _allies.Add(hit.transform.GetComponent<EnemyController>());
            }
        }
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

        if (_askForHelpTime <= 0 && !_askedForHelp)
        {
            foreach (EnemyController ally in _allies)
            {
                ally.AskForHelp();
            }
            _askedForHelp = true;
        }
        else
        {
            _askForHelpTime -= Time.deltaTime;
        }
    }
    #endregion
}
