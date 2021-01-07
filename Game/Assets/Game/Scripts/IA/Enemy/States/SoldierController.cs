using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using TMPro;
public class SoldierController : EnemyController
{
    #region Variables
    [SerializeField] private float velocity = 5;
    [SerializeField] private float distanceToAttack = 5;
    [SerializeField] private float pushComboForce = 15;
    [SerializeField] private float timeDefenseless = 1;
    private Animator _anim;
    private Collider _collider;
    #endregion

    #region Properties
    public Transform Player { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Transform Transform => transform;
    public float Velocity => velocity;
    public float TimeDefenseless => timeDefenseless;
    public Action<int> OnComboHitEnding { get; set; }

    private float DistanceFromPlayer => Vector3.Distance(Transform.position,
        Player.transform.position);

    private Vector3 DirectionToPlayer =>
        (Player.position - Transform.position).normalized;

    public bool PlayerIsInRange => DistanceFromPlayer <= distanceToAttack;
    // public bool CanAttack => (PlayerIsInRange && CanSeePlayer() && !IsThereAnObstacleInAttackRange());

    public Transform[] points;
    #endregion



    //// public Transform player;
    //private Transform playerCamera;

    //public LayerMask whatIsGround, whatIsPlayer, whatIsVisualObstacle;

    //private GameObject head;
    //private GameObject rightArm;
    //private GameObject spine;
    //private float spineRotatingMargin = 20;
    //private float currentRotation;

    //// Patrolling
    //public Vector3 walkPoint;
    //public float walkPointRange;
    //bool walkPointSet;

    //// Attacking
    //public float timeBetweenAttacks;
    //bool alreadyAttacked;


    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool ragdolls = true;
    [HideInInspector] public Collider[] rigColliders;
    [HideInInspector] public Rigidbody[] rigRigidbodies;

    [HideInInspector] public Vector3 lastHitDirection;
    [HideInInspector] public float lastHitStrength;
    [HideInInspector] public string lastHitBone;

    public SkinnedMeshRenderer mesh;

    public Material restingMaterial;
    public Material patrollingMaterial;
    public Material deathMaterial;
    public Material alertMaterial;
    public Material persecutionMaterial;

    public int currentPatrollingIndex = 1;

    public Animator stateTextAnimator;
    public TextMeshProUGUI stateText;

    public float lookingForPlayerArea = 20;

    public float normalSpeed = 5;
    public float persecutionSpeed = 10;

    #region Methods
    protected override void Awake()
    {
        base.Awake();

        Agent = GetComponent<NavMeshAgent>();
        // Player = GameObject.FindGameObjectWithTag("Player").transform;
        Player = FindObjectOfType<PlayerMovementOld>().transform;
        // _anim = GetComponent<Animator>();
        // _propeller = new AgentPropeller(Agent);
        _collider = GetComponent<Collider>();

        rb = GetComponent<Rigidbody>();

        if (ragdolls)
        {
            rigColliders = GetComponentsInChildren<Collider>();
            rigRigidbodies = GetComponentsInChildren<Rigidbody>();
            StopRagdolls();
            GetComponent<Collider>().isTrigger = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.SetInitialState(new PatrollingState(this, stateMachine, 1));
        transform.forward = DirectionToPlayer;

        
    }

    protected override void Update()
    {
        base.Update();

        // print(Agent.isStopped);


        

        //if (Physics.CheckSphere(transform.position, 2.6f, 1<<25))
        //{
        //    
        //}
        // _propeller.Update(Time.deltaTime);
    }

    protected override void CheckPlayerDetected(Vector3 playerPos, LayerMask playerLayer)
    {
        base.CheckPlayerDetected(playerPos, playerLayer);
    }

    public override void Alert(Vector3 position)
    {
        base.Alert(position);
        stateMachine.SetState(new AlertState(this, stateMachine, position));
    }

    public override void DetectPlayer()
    {
        // base.DetectPlayer();
        // print("PlayerDetected: " + playerDetected);
        if(!playerDetected)
            stateMachine.SetState(new AskForHelpState(this, stateMachine));
    }

    public override void AskForHelp()
    {
        base.AskForHelp();
        if (!playerDetected)
            stateMachine.SetState(new PersecutionState(this, stateMachine));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, distanceToAttack);

        //Gizmos.color = Color.blue;
        //var position = Transform.position;
        //var forward = Transform.forward;
        //Gizmos.DrawLine(position, position + Quaternion.Euler(0, detectAngle / 2, 0) * forward * distanceToAttack);
        //Gizmos.DrawLine(position, position + Quaternion.Euler(0, -detectAngle / 2, 0) * forward * distanceToAttack);

        Gizmos.DrawWireSphere(transform.position, lookingForPlayerArea);
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

    private bool IsThereAnObstacleInAttackRange()
    {

        if (!Physics.Raycast(Transform.position, DirectionToPlayer, out var hitInfo,
            distanceToAttack, LayerMask.GetMask("Ground", "Player")))
        {
            return false;
        }
        return !hitInfo.collider.CompareTag("Player");
    }

    //private bool CanSeePlayer()
    //{
    //    //Vector2 forward = new Vector2(transform.forward.x, Transform.forward.z);
    //    //var playerPosition = Player.position;
    //    //var selfPosition = Transform.position;

    //    //Vector2 toPlayer = new Vector2(playerPosition.x - selfPosition.x,
    //    //    playerPosition.z - selfPosition.z);

    //    //return (Vector2.Angle(forward, toPlayer) < (detectAngle / 2));
    //}

    private void SetNextComboAnimationID(int hitIndex)
    {
        OnComboHitEnding?.Invoke(hitIndex);
    }

    protected override void Die()
    {
        base.Die();

        gameObject.layer = LayerMask.NameToLayer("DeathEnemy");
        _collider.enabled = false;
        Agent.enabled = false;
        _anim.enabled = false;
        // weapon.Disable();
        enabled = false;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health > 0)
        {
            // stateMachine.SetState(new HurtState(this, stateMachine, _anim));
            
        }
        else
        {
            stateMachine.SetState(new DeathState(this, stateMachine));
        }
    }

    public void PushDamage(float damage, Vector3 hitDirection, float hitStrength, string bone)
    {
        lastHitDirection = hitDirection;
        lastHitStrength = hitStrength;
        lastHitBone = bone;
        TakeDamage(damage);
    }

    private void InitAttackInWeapon()
    {
        // weapon.InitAttack();
    }

    private void FinishAttackInWeapon()
    {
        // weapon.FinishAttack();
    }
    #endregion
}
