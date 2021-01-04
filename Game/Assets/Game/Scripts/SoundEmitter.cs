using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundEmitter
{
    public static bool SpawnSoundSphere(Vector3 position, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, 9, 1<<16);

        bool enemyDetected = false;

        if (hitColliders.Length > 0)
        {
            enemyDetected = true;
        }

        for (int i = 0; i < hitColliders.Length; i++)
        {
            SoldierController enemy = hitColliders[i].GetComponent<SoldierController>();
            if (!enemy.playerDetected)
            {
                enemy.Alert(position);
                // enemy.DetectPlayer();
            }

            /// Aquí va el cálculo de la distancia
        }

        return enemyDetected;
    }

    public static bool SpawnSoundSphere(Vector3 position, float alertRadius, float persecutionRadius)
    {
        Collider[] persecutionHitColliders = Physics.OverlapSphere(position, persecutionRadius, 1 << 16);

        for (int i = 0; i < persecutionHitColliders.Length; i++)
        {
            SoldierController enemy = persecutionHitColliders[i].GetComponent<SoldierController>();
            if (!enemy.playerDetected)
            {
                enemy.DetectPlayer();
                return true;
                // enemy.DetectPlayer();
            }
        }

        Collider[] hitColliders = Physics.OverlapSphere(position, alertRadius, 1 << 16);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            SoldierController enemy = hitColliders[i].GetComponent<SoldierController>();
            if (!enemy.playerDetected)
            {
                enemy.Alert(position);
                return true;
                // enemy.DetectPlayer();
            }

            
            /// Aquí va el cálculo de la distancia
        }

        

        return false;
    }


    public static bool SpawnSoundCapsule(Vector3 startPosition, Vector3 finalPosition, float radius)
    {
        bool enemyDetected = false;

        Collider[] hitColliders = Physics.OverlapCapsule(startPosition, finalPosition, radius, 1 << 16);

        if (hitColliders.Length > 0)
        {
            enemyDetected = true;
        }

        for (int i = 0; i < hitColliders.Length; i++)
        {
            SoldierController enemy = hitColliders[i].GetComponent<SoldierController>();
            if (!enemy.playerDetected)
            {
                enemy.DetectPlayer();
            }
        }

        return enemyDetected;
    }
}
