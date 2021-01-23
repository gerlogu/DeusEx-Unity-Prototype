using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Clase que representa el estado de "persecución" 
/// del dron. Entra en este estado si el visor asociado
/// al mismo detecta al jugador.
/// </summary>

public class DronePersecutionState : State
{
    #region Variables
    private readonly EnemyDroneController _enemyController;
    private float lostLocationTimer;
    private Vector3 playerLocation;
    private float _timeToShoot;
    #endregion

    #region Methods
    public DronePersecutionState(EnemyDroneController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;
        lostLocationTimer = 5;
        _enemyController.mesh.material = _enemyController.persecutionMaterial;

        _enemyController.playerDetected = true;
        _enemyController.Agent.speed = _enemyController.persecutionSpeed;
        _enemyController.Agent.stoppingDistance = 10f;
        _enemyController.Agent.SetDestination(_enemyController.Player.position);
        playerLocation = _enemyController.Player.position;
        _timeToShoot = 1;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (Physics.CheckSphere(_enemyController.transform.position, _enemyController.lookingForPlayerArea, 1 << 10))
        {
            if (_enemyController.CheckPlayerVision(_enemyController.player.transform.position, 10))
            {
                _enemyController.Agent.SetDestination(_enemyController.player.position);
                lostLocationTimer = Random.Range(2.5f, 4.5f);
                playerLocation = _enemyController.player.transform.position;
                Quaternion currentRot = Quaternion.LookRotation(_enemyController.player.position - _enemyController.transform.position);
                _enemyController.transform.rotation = Quaternion.Lerp(_enemyController.transform.rotation, currentRot, 8 * Time.deltaTime);
            }

            if (_timeToShoot <= 0)
            {
                GameObject shot = GameObject.Instantiate(_enemyController.shotPrefab, _enemyController.weapon.position, _enemyController.weapon.rotation);
                shot.transform.LookAt(_enemyController.player);
                _timeToShoot = Random.Range(1, 3);
                Debug.Log("Shoot");
            }
            else
            {
                _timeToShoot -= Time.deltaTime;
            }
        }

        if (lostLocationTimer <= 0)
        {
            stateMachine.SetState(new DronePatrollingState(_enemyController, stateMachine, _enemyController.currentPatrollingIndex));
        }
        else if (Vector3.Distance(_enemyController.transform.position, playerLocation) <= _enemyController.Agent.stoppingDistance || (_enemyController.Agent.pathStatus == NavMeshPathStatus.PathInvalid || _enemyController.Agent.pathStatus == NavMeshPathStatus.PathPartial))
        {
            lostLocationTimer -= Time.deltaTime;
        }
    }

    #endregion
}
