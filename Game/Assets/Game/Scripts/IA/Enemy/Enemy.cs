using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    public bool ragdolls = false;
    protected Collider[] rigColliders;
    protected Rigidbody[] rigRigidbodies;
    public Rigidbody pelvis;
    protected int currentHealth;
    public Animator anim;
    public GameObject weapon;
    public Posture posture;
    public GameObject visor;
    public enum Posture
    {
        IDLE,
        WALL
    }

    protected SoundManager soundManager;
    protected AudioSource audioSource;
    protected Rigidbody rb;
    public bool isDead = false;
    public bool collisionsDisabled = false;
    protected bool pushable = true;

    public delegate void Damaged(int damage);
    public Damaged OnReceiveDamage;
    NavMeshAgent agent;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        currentHealth = health;
        agent = GetComponent<NavMeshAgent>();

        switch (posture)
        {
            case Posture.IDLE:
                anim.SetTrigger("Idle");
                break;
            case Posture.WALL:
                anim.SetTrigger("Wall");
                break;
        }

        if (ragdolls)
        {
            rigColliders = GetComponentsInChildren<Collider>();
            rigRigidbodies = GetComponentsInChildren<Rigidbody>();
            StopRagdolls();
            GetComponent<Collider>().isTrigger = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }

        OnReceiveDamage = (int damage) =>
        {

            if (!isDead)
            {
                soundManager.enemy.PlaySFX(audioSource, 0);
            }
                
            if (currentHealth <= 0)
            {
                if (FindObjectOfType<Gun>() && !isDead)
                {
                    if (anim)
                    {
                        anim.enabled = false;
                    }
                    
                    FindObjectOfType<TimeScaleManager>().SlowMotionKill();
                    isDead = true;
                }
                Die();

            }
        };
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isDead) {
            if (agent.enabled)
            {
                agent.enabled = false;
            }
                
        }
        
    }

    public void Push(Vector3 dir, float push)
    {
        if (isDead && pushable)
        {
            rb.AddForce(dir * push, ForceMode.Impulse);
            if (ragdolls)
            {
                

                foreach (Collider col in rigColliders)
                {
                    col.isTrigger = false;
                }

                foreach (Rigidbody rb in rigRigidbodies)
                {
                    rb.isKinematic = false;
                }
                GetComponent<Collider>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                if (pelvis)
                {
                    pelvis.AddForce(dir * push * 75, ForceMode.Impulse);
                    Invoke("StopRagdolls", 5f);
                }
            }
            weapon.transform.parent = null;
            pushable = false;
            tag = "DeadEnemy";
            gameObject.layer = 18;
            DisableCollisions(this.gameObject);
            collisionsDisabled = true;
        }
    }

    public void Push(Vector3 dir, float push, string boneName)
    {
        if (isDead && pushable)
        {
            //rb.AddForce(dir * push, ForceMode.Impulse);
            if (ragdolls)
            {
                foreach (Collider col in rigColliders)
                {
                    col.isTrigger = false;
                }

                foreach (Rigidbody rb in rigRigidbodies)
                {
                    rb.isKinematic = false;
                }
                GetComponent<Collider>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                foreach(Collider col in rigColliders)
                {
                    if(col.name == boneName)
                    {
                        col.GetComponent<Rigidbody>().AddForce(dir * push * 75, ForceMode.Impulse);
                        Invoke("StopRagdolls", 5f);
                    }
                }
            }

            pushable = false;
            tag = "DeadEnemy";
            gameObject.layer = 18;
            DisableCollisions(this.gameObject);
            collisionsDisabled = true;
        }
    }

    void StopRagdolls()
    {
        foreach (Collider col in rigColliders)
        {
            col.isTrigger = true;
        }

        foreach (Rigidbody rb in rigRigidbodies)
        {
            rb.isKinematic = true;
        }
    }

    public void DisableCollisions(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; ++i)
        {
            parent.transform.GetChild(i).gameObject.layer = 18;
            if (transform.GetChild(i).transform.childCount != 0)
            {
                DisableCollisions(transform.GetChild(i).gameObject);
            }
        }
    }


    public virtual void Damage(int damage)
    {
        currentHealth -= damage;
        OnReceiveDamage(damage);
    }

    public virtual void Die()
    {
        if(visor)
            visor.SetActive(false);
        if(GetComponent<EnemyAI_Human>())
            GetComponent<EnemyAI_Human>().enabled = false;
        rb.constraints = RigidbodyConstraints.None;
        // GetComponent<NavMeshAgent>().enabled = false;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                // col.isTrigger = false;
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(col.transform.position, 0.1f);
            }
        }
    }
    #endif
}