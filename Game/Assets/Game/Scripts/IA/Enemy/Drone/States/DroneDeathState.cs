using UnityEngine;

/// <summary>
/// Clase que representa el estado de "muerte" 
/// del dron. Entra en este estado cuando el 
/// jugador le acierta un disparo y lo derriba.
/// </summary>

public class DroneDeathState : State
{
    #region Variables
    private readonly EnemyDroneController _enemyController;
    #endregion

    #region Methods
    public DroneDeathState(EnemyDroneController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;

        _enemyController.mesh.material = _enemyController.deathMaterial;

        if (_enemyController.visor)
            _enemyController.visor.gameObject.SetActive(false);
        if (_enemyController.GetComponent<EnemyAI_Human>())
            _enemyController.GetComponent<EnemyAI_Human>().enabled = false;
        _enemyController.rb.constraints = RigidbodyConstraints.None;

        _enemyController.GetComponentInChildren<Animator>().enabled = false;

        _enemyController.isDead = true;
    }
    #endregion
}
