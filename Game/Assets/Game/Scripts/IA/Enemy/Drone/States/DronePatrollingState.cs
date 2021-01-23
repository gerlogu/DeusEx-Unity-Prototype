using UnityEngine;

public class DronePatrollingState : State
{

    private readonly EnemyDroneController _enemyController;
    private int _patrollingIndex;


    public DronePatrollingState(EnemyDroneController enemyController, MyStateMachine stateMachine, int patrollingIndex) : base(stateMachine)
    {
        _patrollingIndex = patrollingIndex;
        _enemyController = enemyController;
        _enemyController.mesh.material = _enemyController.patrollingMaterial;

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
                    Debug.Log("Location Reached.");
                }
            }
        }
    }
}
