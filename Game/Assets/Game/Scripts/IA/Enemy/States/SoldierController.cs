using UnityEngine;
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

    public Transform[] points;
    #endregion

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

    public GameObject shotPrefab;

    public Transform weapon;

    #region Methods
    protected override void Awake()
    {
        base.Awake();

        Agent = GetComponent<NavMeshAgent>();
        Player = FindObjectOfType<PlayerMovement>().transform;
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
    }

    protected override void Update()
    {
        base.Update();
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

    protected override void Die()
    {
        base.Die();

        gameObject.layer = LayerMask.NameToLayer("DeathEnemy");
        _collider.enabled = false;
        Agent.enabled = false;
        _anim.enabled = false;
        enabled = false;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health <= 0)
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
    #endregion
}
