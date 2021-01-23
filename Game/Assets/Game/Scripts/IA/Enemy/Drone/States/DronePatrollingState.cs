using UnityEngine;

/// <summary>
/// Clase que representa el estado de "patrulla" 
/// del dron. Es su estado por defecto, en el que
/// comienza al empezar la partida. Realiza un 
/// recorrido en base a una serie de puntos claves.
/// </summary>

public class DronePatrollingState : State
{
    #region Variables
    private readonly EnemyDroneController _enemyController;
    private int _patrollingIndex;
    private float _timeToRest;
    #endregion

    #region Methods
    public DronePatrollingState(EnemyDroneController enemyController, MyStateMachine stateMachine, int patrollingIndex) : base(stateMachine)
    {
        _patrollingIndex = patrollingIndex;
        _enemyController = enemyController;
        _enemyController.mesh.material = _enemyController.patrollingMaterial;
        _timeToRest = Random.Range(3, 6);
        _enemyController.Agent.speed = _enemyController.normalSpeed;
        _enemyController.playerDetected = false;
        _enemyController.Agent.stoppingDistance = 1f;
        _enemyController.currentPatrollingIndex++;

        if (_enemyController.currentPatrollingIndex >= _enemyController.points.Length)
        {
            _enemyController.currentPatrollingIndex = 0;
        }
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
                    if (_timeToRest <= 0)
                    {
                        Debug.Log("Hiiii");
                        stateMachine.SetState(new DronePatrollingState(_enemyController, stateMachine, _enemyController.currentPatrollingIndex));
                    }
                    else
                    {
                        _timeToRest -= Time.deltaTime;
                    }
                }
            }
        }
    }
    #endregion
}
