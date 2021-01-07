using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

        //rb.AddForce(dir * push, ForceMode.Impulse);
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
                    // Invoke("StopRagdolls", 5f);
                }
            }
        }
        Debug.Log(_enemyController.lastHitDirection);
        Debug.Log(_enemyController.lastHitStrength);
        Debug.Log(_enemyController.lastHitBone);

        _enemyController.isDead = true;

        //pushable = false;
        //tag = "DeadEnemy";
        //gameObject.layer = 18;
        //DisableCollisions(this.gameObject);
        //collisionsDisabled = true;
    }
    #endregion
}
