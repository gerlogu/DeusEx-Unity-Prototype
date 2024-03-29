﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que representa el estado de "pedir ayuda" 
/// del enemigo humano. Entra en este estado justo
/// cuando el soldado detecta al jugador y comienza
/// la persecución. Antes de perseguirle, da la alarma
/// a sus aliados en un radio cercano.
/// </summary>

public class AskForHelpState : State
{
    #region Variables
    private readonly SoldierController _enemyController;
    private float _askForHelpTime;
    private List<EnemyController> _allies;
    #endregion

    #region Method
    public AskForHelpState(SoldierController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;
        _askForHelpTime = 0.5f;
        _enemyController.mesh.material = _enemyController.persecutionMaterial;

        _enemyController.stateTextAnimator.SetBool("Alert", false);
        _enemyController.stateTextAnimator.SetBool("Persecution", true);
        _enemyController.stateTextAnimator.SetBool("None", false);
        _enemyController.stateText.text = "!";
        _enemyController.playerDetected = true;
        _enemyController.Agent.speed = 0;
        _enemyController.Agent.stoppingDistance = 10f;

        _allies = new List<EnemyController>();
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(_enemyController.transform.position, 40, _enemyController.player.transform.up, 1<<16);

        foreach(RaycastHit hit in hits)
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
            if(_enemyController.CheckPlayerVision(_enemyController.player.transform.position, 10))
            {
                Quaternion currentRot = Quaternion.LookRotation(_enemyController.player.position - _enemyController.transform.position);
                _enemyController.transform.rotation = Quaternion.Lerp(_enemyController.transform.rotation, currentRot, 8 * Time.deltaTime);
            }
            
        }

        if (_askForHelpTime <= 0)
        {
            foreach(EnemyController ally in _allies)
            {
                ally.AskForHelp();
            }
            stateMachine.SetState(new PersecutionState(_enemyController, stateMachine));
        }
        else
        {
            _askForHelpTime -= Time.deltaTime;
        }

       
    }
    #endregion
}
