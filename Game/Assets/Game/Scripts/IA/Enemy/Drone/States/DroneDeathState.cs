using UnityEngine;

public class DroneDeathState : State
{
    private readonly EnemyDroneController _enemyController;

    public DroneDeathState(EnemyDroneController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;

        _enemyController.mesh.material = _enemyController.deathMaterial;

        if (_enemyController.visor)
            _enemyController.visor.gameObject.SetActive(false);
        if (_enemyController.GetComponent<EnemyAI_Human>())
            _enemyController.GetComponent<EnemyAI_Human>().enabled = false;

        _enemyController.rb.constraints = RigidbodyConstraints.None;
        _enemyController.rb.isKinematic = false;
        _enemyController.rb.gameObject.layer = 18;
        _enemyController.Agent.isStopped = true;

        _enemyController.isDead = true;
    }
}
