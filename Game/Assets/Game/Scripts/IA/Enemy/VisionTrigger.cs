using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionTrigger : MonoBehaviour
{
    public delegate void PlayerDetected(Vector3 playerPos, LayerMask playerLayer);
    public PlayerDetected OnPlayerDetected;

    public delegate void PlayerUndetected();
    public PlayerUndetected OnPlayerUndetected;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // print("Player");
            OnPlayerDetected(other.transform.position, other.gameObject.layer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerUndetected();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.8f, 0, 0, 0.25f);
        Mesh mesh = this.GetComponent<MeshFilter>().sharedMesh;
        Gizmos.DrawMesh(mesh, this.transform.position, this.transform.rotation);
    }
}
