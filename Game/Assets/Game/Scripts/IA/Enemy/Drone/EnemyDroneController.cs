using UnityEngine.AI;
using UnityEngine;

/// <summary>
/// Clase que representa la máquina de estados que 
/// controla el comportamiento del tipo de enemigo:
/// dron. Gestiona el cambio de estados en base a las
/// condiciones previamente establecidas.
/// </summary>

public class EnemyDroneController : EnemyController
{
    [HideInInspector] public Vector3 lastHitDirection;
    [HideInInspector] public float lastHitStrength;
    [HideInInspector] public string lastHitBone;

    public NavMeshAgent Agent { get; private set; }
    public Transform Player { get; private set; }

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
    public GameObject shotPrefab;
    public Transform weapon;

    #region Method
    protected override void Awake()
    {
        base.Awake();

        Agent = GetComponent<NavMeshAgent>();
        Player = FindObjectOfType<PlayerMovement>().transform;

        
        rb = GetComponent<Rigidbody>();

        if (!rb)
        {
            rb = GetComponentInChildren<Rigidbody>();
        }
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.SetInitialState(new DronePatrollingState(this, stateMachine, 1));
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

        if (health <= 0)
        {
            stateMachine.SetState(new DroneDeathState(this, stateMachine));
        }
    }

    public void PushDamage(float damage, Vector3 hitDirection, float hitStrength, string bone)
    {
        lastHitDirection = hitDirection;
        lastHitStrength = hitStrength;
        lastHitBone = bone;
        TakeDamage(damage);
    }

    #endregion
}
