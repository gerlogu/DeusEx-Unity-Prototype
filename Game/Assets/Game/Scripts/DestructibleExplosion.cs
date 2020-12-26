using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Explosive element main script.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class DestructibleExplosion : MonoBehaviour
{
    [Tooltip("Min explosion force")]
    [SerializeField] private float minForce;
    [Tooltip("Max explosion force")]
    [SerializeField] private float maxForce;
    [Tooltip("Explosion radius (just for physics)")]
    [SerializeField] private float physicsRadius = 5;
    [Tooltip("Explosion damage radius")]
    [SerializeField] private float damageRadius = 5.5f;
    [Tooltip("Ignored Layer")]
    [SerializeField] private LayerMask whatIsIgnored;
    [Tooltip("Explosion prefab")]
    [SerializeField] private GameObject explosion;
    [Tooltip("Physics push force (for enemies)")]
    [SerializeField] private float pushForce = 50;
    [Tooltip("Explosion Location")]
    [SerializeField] private Transform explosionBase;

    private SoundManager soundManager; // Sound Manager
    private AudioSource audioSource;   // Audio Source
    private bool exploded = false;     // Determines if object has already exploded

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Makes the explosive object to explode.
    /// </summary>
    public void Explode()
    {
        if (exploded)
            return;

        if (!soundManager)
        {
            soundManager = FindObjectOfType<SoundManager>();
        }

        soundManager.gameElements.PlaySFX(audioSource, GameElementsSound.BARREL_EXPLOSION);

        foreach (Transform t in transform)
        {
            Rigidbody rb = t.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position - new Vector3(0,0,0), physicsRadius);
            }
        }

        Invoke("CheckIfDamageEnemies", .025f); // Checks if enemies are damaged by the explosion

        Instantiate(explosion, transform.position, new Quaternion(0,0,0,0));

        exploded = true;
    }

    /// <summary>
    /// Checks if enemies are damaged by the explosion.
    /// </summary>
    void CheckIfDamageEnemies()
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, damageRadius, transform.right, damageRadius);

        RaycastHit rayHit;

        if (hits.Length != 0)
        {

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Enemy") && Physics.Raycast(transform.position, (hit.transform.position - transform.position).normalized, out rayHit, (hit.point - transform.position).magnitude, ~whatIsIgnored))
                {

                    if (rayHit.collider.CompareTag("Enemy"))
                    {
                        Vector3 deathPush = (hit.transform.position - explosionBase.position).normalized;
                        hit.collider.GetComponent<Enemy>().Damage(50);
                        hit.collider.GetComponent<Enemy>().Push(deathPush, pushForce);

                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
