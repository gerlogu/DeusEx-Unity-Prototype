using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Clase que representa el estado de "muerte" 
/// del soldado. Entra en este estado cuando el
/// jugador le acierta un disparo.
/// </summary>

public class DeathState : State
{
    #region Variables
    private readonly SoldierController _enemyController;
    #endregion

    #region Methods
    public DeathState(SoldierController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;

        _enemyController.mesh.material = _enemyController.deathMaterial;

        if (_enemyController.visor)
            _enemyController.visor.gameObject.SetActive(false);

        if (_enemyController.GetComponent<EnemyAI_Human>())
            _enemyController.GetComponent<EnemyAI_Human>().enabled = false;

        _enemyController.rb.constraints = RigidbodyConstraints.None;

        _enemyController.GetComponent<NavMeshAgent>().enabled = false;
        _enemyController.GetComponentInChildren<Animator>().enabled = false;

        if (_enemyController.ragdolls)
        {
            foreach (Collider col in _enemyController.rigColliders)
            {
                col.isTrigger = false;
            }

            foreach (Rigidbody rb in _enemyController.rigRigidbodies)
            {
                rb.isKinematic = false;
            }

            _enemyController.GetComponent<Collider>().enabled = false;
            _enemyController.GetComponent<Rigidbody>().isKinematic = true;

            foreach (Collider col in _enemyController.rigColliders)
            {
                if (col.name == _enemyController.lastHitBone)
                {
                    col.GetComponent<Rigidbody>().AddForce(_enemyController.lastHitDirection * _enemyController.lastHitStrength * 75, ForceMode.Impulse);
                }
            }
        }

        _enemyController.isDead = true;
    }
    #endregion
}
