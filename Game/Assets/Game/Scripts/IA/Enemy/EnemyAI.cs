using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : Enemy
{
    

    public NavMeshAgent agent;

    public Transform player;
    private Transform playerCamera;

    public LayerMask whatIsGround, whatIsPlayer, whatIsVisualObstacle;

    private GameObject head;
    private GameObject rightArm;
    private GameObject spine;
    private float spineRotatingMargin = 20;
    private float currentRotation;

    // Patrolling
    public Vector3 walkPoint;
    public float walkPointRange;
    bool walkPointSet;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // States
    public float sightRange = 27.87f, attackRange = 17.12f, ignoreRange = 46f;
    public bool playerInSightRange, playerInAttackRange, playerInIgnoreRange;

    private void Awake()
    {
        if(!player)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        if(!agent)
            agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        foreach (Collider col in rigColliders)
        {
            if (col.CompareTag("EnemyHeadHitbox"))
            {
                head = col.gameObject;
            }
            if (col.gameObject.name == "bicep.R")
            {
                rightArm = col.gameObject;
            }
            if(col.gameObject.name == "spine2")
            {
                spine = col.gameObject;
            }
        }
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    Quaternion newRotation;
    bool canRotateArms = false;
    protected override void Update()
    {
        base.Update();

        if (isDead)
        {
            if(agent.enabled)
                agent.enabled = false;
            return;
        }

        if (playerDetected)
        {
            anim.enabled = false;
            head.transform.LookAt(playerCamera);
            head.transform.rotation = Quaternion.Euler(head.transform.rotation.eulerAngles.x + 10, head.transform.rotation.eulerAngles.y, head.transform.rotation.eulerAngles.z);
            if (canRotateArms)
            {
                //rightArm.transform.LookAt(playerCamera);
                //rightArm.transform.rotation = Quaternion.Euler(rightArm.transform.rotation.eulerAngles.x - 69, rightArm.transform.rotation.eulerAngles.y - 7, rightArm.transform.rotation.eulerAngles.z - 170);
                //spine.transform.LookAt(playerCamera);
            }

            Quaternion armRotation = Quaternion.LookRotation(playerCamera.position - rightArm.transform.position);
            rightArm.transform.rotation = Quaternion.Slerp(rightArm.transform.rotation, Quaternion.Euler(armRotation.eulerAngles.x - 69, armRotation.eulerAngles.y - 7, armRotation.eulerAngles.z - 170), Time.deltaTime * 8);

            Quaternion spineRotation = Quaternion.LookRotation(playerCamera.position - spine.transform.position);
            spine.transform.rotation = Quaternion.Slerp(spine.transform.rotation, Quaternion.Euler(spineRotation.eulerAngles.x, spineRotation.eulerAngles.y, spineRotation.eulerAngles.z), Time.deltaTime * 8);

            Vector3 targetDir = player.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);
            
            if(angle > currentRotation + 15 || angle < currentRotation - 15)
            {
                currentRotation = angle;
                newRotation = Quaternion.LookRotation(player.position - transform.position);
                canRotateArms = false;
            }
        }
        

        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInIgnoreRange = Physics.CheckSphere(transform.position, ignoreRange, whatIsPlayer);


        if (!playerInSightRange && !playerInAttackRange && !playerDetected)
        {
            // Patrolling();
            agent.isStopped = true;
        }
        
        if (!Physics.Raycast(transform.position, (player.position - transform.position).normalized, (player.position - transform.position).magnitude, whatIsVisualObstacle) && playerInSightRange && !playerDetected)
        {
            playerDetected = true;
            Vector3 targetDir = player.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);
            currentRotation = angle;
            newRotation = Quaternion.LookRotation(player.position - transform.position);
            //print("Player Detected");
        }
        else if(!playerInIgnoreRange)
        {
            // print("Looking for Player");
            playerDetected = false;
        }

        #if UNITY_EDITOR
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, sightRange, whatIsVisualObstacle) )
        {
            Debug.DrawRay(transform.position, (hit.point - transform.position).normalized * (hit.point - transform.position).magnitude, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, (player.position - transform.position).normalized * (player.position - transform.position).magnitude, Color.red);
        }
        #endif

        if (!playerDetected)
        {
            return;
        }

        #region Player Detected
        pelvis.transform.rotation = Quaternion.Slerp(pelvis.transform.rotation, Quaternion.Euler(pelvis.transform.rotation.eulerAngles.x, newRotation.eulerAngles.y, pelvis.transform.rotation.eulerAngles.z), Time.deltaTime * 5);
        #endregion

        if (playerInSightRange && !playerInAttackRange)
        {
            Vector3 distance = player.position - transform.position;
            Vector3 rot;
            //ChasePlayer();
        }
        else if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }

        //if (!playerInIgnoreRange)
        //{
            
        //}
    }



    #region Patrolling
    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        Vector3 random = new Vector3(Random.Range(-walkPointRange, walkPointRange), 0, Random.Range(-walkPointRange, walkPointRange));

        walkPoint = new Vector3(transform.position.x + random.x, transform.position.y + random.y, transform.position.z + random.z);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    #endregion

    bool playerDetected = false;

    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }
    Vector3 targetPos;
    private void AttackPlayer()
    {
        agent.isStopped = true;
        if (!alreadyAttacked)
        {
            /// Attack code here
            
            /// 
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public override void Die()
    {
        base.Die();
        GetComponent<NavMeshAgent>().enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, ignoreRange);
    }
}
