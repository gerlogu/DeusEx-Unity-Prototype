using System.Collections.Generic;
using UnityEngine;

public class DroneAskForHelpState : State
{
    #region Variables
    private readonly EnemyDroneController _enemyController;
    private float _askForHelpTime;
    private List<EnemyController> _allies;
    #endregion

    public DroneAskForHelpState(EnemyDroneController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;
        _askForHelpTime = 0.5f;
        _enemyController.mesh.material = _enemyController.persecutionMaterial;

        _enemyController.playerDetected = true;
        _enemyController.Agent.speed = 0;
        _enemyController.Agent.stoppingDistance = 10f;

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

        if (Physics.CheckSphere(_enemyController.transform.position, _enemyController.lookingForPlayerArea, 1 << 10))
        {
            if (_enemyController.CheckPlayerVision(_enemyController.player.transform.position, 10))
            {
                Quaternion currentRot = Quaternion.LookRotation(_enemyController.player.position - _enemyController.transform.position);
                _enemyController.transform.rotation = Quaternion.Lerp(_enemyController.transform.rotation, currentRot, 8 * Time.deltaTime);
            }
        }

        if (_askForHelpTime <= 0)
        {
            foreach (EnemyController ally in _allies)
            {
                ally.AskForHelp();
            }
            stateMachine.SetState(new DronePersecutionState(_enemyController, stateMachine));
        }
        else
        {
            _askForHelpTime -= Time.deltaTime;
        }

    }
}
