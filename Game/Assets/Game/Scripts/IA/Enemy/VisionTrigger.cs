using UnityEngine;

public class VisionTrigger : MonoBehaviour
{
    public delegate void PlayerDetected(Vector3 playerPos, LayerMask playerLayer);
    public PlayerDetected OnPlayerDetected;

    public delegate void PlayerUndetected();
    public PlayerUndetected OnPlayerUndetected;

    private Color color = new Color(0, 0.3f, 0.8f, 0.25f);

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            color = new Color(0.8f, 0, 0, 0.25f);
            OnPlayerDetected(other.transform.position, other.gameObject.layer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            color = new Color(0, 0.3f, 0.8f, 0.25f);
            OnPlayerUndetected();
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = new Color(0, 0.3f, 0.8f, 0.25f);
        }
        else
        {
            Gizmos.color = color;
        }
        
        Mesh mesh = this.GetComponent<MeshFilter>().sharedMesh;
        Gizmos.DrawMesh(mesh, this.transform.position, this.transform.rotation);
    }
}
