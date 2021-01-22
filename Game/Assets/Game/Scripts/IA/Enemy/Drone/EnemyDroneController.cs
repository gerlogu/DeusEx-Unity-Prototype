using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyDroneController : EnemyController
{

    private Collider _collider;
    private Collider _droneCollider;


    [HideInInspector] public Vector3 lastHitDirection;
    [HideInInspector] public float lastHitStrength;
    [HideInInspector] public string lastHitBone;

    public NavMeshAgent Agent { get; private set; }
    public Transform Player { get; private set; }
    public Transform Transform => transform;

    private Vector3 DirectionToPlayer =>
    (Player.position - Transform.position).normalized;

    [HideInInspector] public Rigidbody rb;

    public MeshRenderer mesh;

    public Material patrollingMaterial;
    public Material deathMaterial;
    public Material persecutionMaterial;

    public float normalSpeed = 5;
    public float persecutionSpeed = 10;

    public float lookingForPlayerArea = 10;
    public int currentPatrollingIndex = 1;

    public Transform[] points;

    #region Method

    protected override void Awake()
    {
        base.Awake();

        Agent = GetComponent<NavMeshAgent>();
        Player = FindObjectOfType<PlayerMovementOld>().transform;
        _collider = GetComponent<Collider>();
        _droneCollider = GetComponentInChildren<BoxCollider>();

        rb = GetComponent<Rigidbody>();

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.SetInitialState(new DronePatrollingState(this, stateMachine, 1));
        transform.forward = DirectionToPlayer;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void CheckPlayerDetected(Vector3 playerPos, LayerMask playerLayer)
    {
        base.CheckPlayerDetected(playerPos, playerLayer);
    }

    public override void DetectPlayer()
    {
        print("DRONE PlayerDetected: " + playerDetected);
        if (!playerDetected)
            stateMachine.SetState(new DroneAskForHelpState(this, stateMachine));
    }

    public override void AskForHelp()
    {
        base.AskForHelp();
        if (!playerDetected)
            stateMachine.SetState(new DronePersecutionState(this, stateMachine));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookingForPlayerArea);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health > 0)
        {

        }
        else
        {
            stateMachine.SetState(new DroneDeathState(this, stateMachine));
        }
    }

    public void PushDamage(float damage, Vector3 hitDirection, float hitStrength, string bone)
    {

        print("<color=cyan>Drone ready to die!</color>");

        //if (_droneCollider)
        //{
        //    print("<color=red>Drone dying!</color>");
        //    stateMachine.SetState(new DroneDeathState(this, stateMachine));
        //}

        lastHitDirection = hitDirection;
        lastHitStrength = hitStrength;
        lastHitBone = bone;
        TakeDamage(damage);
    }

    #endregion
}
