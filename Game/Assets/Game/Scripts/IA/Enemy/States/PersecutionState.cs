using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Clase que representa el estado de "persecución" 
/// del soldado. Entra en este estado si el visor asociado
/// a la misma detecta al jugador o si lo escucha a una
/// distancia lo suficientemente cercana como para
/// reconocerlo como una amenaza.
/// </summary>

public class PersecutionState : State
{
    #region Variables
    private readonly SoldierController _enemyController;
    private float _lostLocationTimer;
    private Vector3 _playerLocation;
    private float _timeToShoot;
    #endregion

    #region Methods
    public PersecutionState(SoldierController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;
        _lostLocationTimer = 5;
        _enemyController.mesh.material = _enemyController.persecutionMaterial;

        _enemyController.stateTextAnimator.SetBool("Alert", false);
        _enemyController.stateTextAnimator.SetBool("Persecution", true);
        _enemyController.stateTextAnimator.SetBool("None", false);
        _enemyController.stateText.text = "!";
        _enemyController.playerDetected = true;
        _enemyController.Agent.speed = _enemyController.persecutionSpeed;
        _enemyController.Agent.stoppingDistance = 10f;
        _enemyController.Agent.SetDestination(_enemyController.Player.position);
        _playerLocation = _enemyController.Player.position;
        _timeToShoot = 1;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (Physics.CheckSphere(_enemyController.transform.position, _enemyController.lookingForPlayerArea, 1 << 10))
        {
            if(_enemyController.CheckPlayerVision(_enemyController.player.transform.position, 10))
            {
                _enemyController.Agent.SetDestination(_enemyController.player.position);
                _lostLocationTimer = Random.Range(2.5f,4.5f);
                _playerLocation = _enemyController.player.transform.position;
                
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

        if (_lostLocationTimer <= 0)
        {
            stateMachine.SetState(new PatrollingState(_enemyController, stateMachine, _enemyController.currentPatrollingIndex));
        }
        else if(Vector3.Distance(_enemyController.transform.position, _playerLocation) <= _enemyController.Agent.stoppingDistance || (_enemyController.Agent.pathStatus == NavMeshPathStatus.PathInvalid || _enemyController.Agent.pathStatus == NavMeshPathStatus.PathPartial))
        {
            _lostLocationTimer -= Time.deltaTime;
        }
    }

    #endregion Methods
}
