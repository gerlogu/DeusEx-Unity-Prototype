using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Human : MonoBehaviour
{
    [SerializeField] private bool playerDetected;   // Jugador Detectado
    [SerializeField] private LayerMask whatIsEnemy; // Lo que es ignorado al detectar al jugador
    private Transform player;                       // Referencia al transform del player
    private Quaternion currentRot;                  // Rotación del enemigo

    // Start is called before the first frame update
    void Start()
    {
        VisionTrigger visionTrigger = GetComponentInChildren<VisionTrigger>();
        if (visionTrigger)
        {
            visionTrigger.OnPlayerDetected += (Vector3 playerPos, LayerMask playerLayer) => CheckPlayerDetected(playerPos, playerLayer);
            visionTrigger.OnPlayerUndetected += UndetectPlayer;
        }
    }

    /// <summary>
    /// Comprueba si el jugador es visible.
    /// </summary>
    /// <param name="playerPos">Posición del jugador</param>
    /// <param name="playerLayer">Layer del jugador</param>
    void CheckPlayerDetected(Vector3 playerPos, LayerMask playerLayer)
    {
        RaycastHit hitToPlayer;
        Vector3 rayDirection = (playerPos - transform.position).normalized;
        float rayDistance = (playerPos - transform.position).magnitude;

        if (Physics.Raycast(transform.position, rayDirection, out hitToPlayer, rayDistance, ~whatIsEnemy))
        {
            if (hitToPlayer.transform.gameObject.layer == playerLayer)
            {
                Debug.Log("Player Detected");
                if (!player)
                    player = hitToPlayer.transform;
                DetectPlayer();
            }
            else
            {
                UndetectPlayer();
            }
        }
    }

    void DetectPlayer()
    {
        if (!playerDetected)
        {
            playerDetected = true;
        }
    }

    void UndetectPlayer()
    {
        Debug.Log("Player Not Detected");
        playerDetected = false;
    }

    private void Update()
    {
        if (playerDetected)
        {
            currentRot = Quaternion.LookRotation(player.position - transform.position);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, currentRot, 8 * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        if (playerDetected)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}
