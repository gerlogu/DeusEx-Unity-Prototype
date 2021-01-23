using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Variables
    public float health;
    public VisionTrigger visor;
    public Transform player;
    public LayerMask whatIsEnemy;
    public bool playerDetected = false;
    public bool isDead = false;
    public bool askedForHelp = false;

    protected MyStateMachine stateMachine;
    #endregion

    #region Methods
    protected virtual void Awake()
    {
        stateMachine = new MyStateMachine();
    }

    protected virtual void Start()
    {
        visor.OnPlayerDetected += (Vector3 playerPos, LayerMask playerLayer) => CheckPlayerDetected(playerPos, playerLayer);
        visor.OnPlayerUndetected += UndetectPlayer;
    }

    public virtual void AskForHelp()
    {

    }

    protected virtual void CheckPlayerDetected(Vector3 playerPos, LayerMask playerLayer)
    {
        RaycastHit hitToPlayer;
        Vector3 rayDirection = (playerPos - transform.position).normalized;
        float rayDistance = (playerPos - transform.position).magnitude;

        if (Physics.Raycast(transform.position, rayDirection, out hitToPlayer, rayDistance, ~whatIsEnemy))
        {
            if (hitToPlayer.transform.gameObject.layer == playerLayer)
            {
                DetectPlayer();
            }
            else
            {
                UndetectPlayer();
            }
        }
    }

    public virtual bool CheckPlayerVision(Vector3 playerPos, int playerLayer)
    {
        RaycastHit hitToPlayer;
        Vector3 rayDirection = (playerPos - transform.position).normalized;
        float rayDistance = (playerPos - transform.position).magnitude;

        if (Physics.Raycast(transform.position, rayDirection, out hitToPlayer, rayDistance + 1000, ~whatIsEnemy))
        {
            if (hitToPlayer.transform.gameObject.layer == playerLayer)
            {
                if (!player)
                    player = hitToPlayer.transform;
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    protected virtual void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.FixedUpdate(Time.deltaTime);
    }

    protected virtual void Die()
    {

    }

    public virtual void DetectPlayer()
    {
        if (!playerDetected)
        {
            playerDetected = true;
        }
    }

    public virtual void Alert(Vector3 position)
    {
        
    }

    public virtual void UndetectPlayer()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        print("<color=cyan>Taking Damage!</color>");
        health -= damage;
    }
    #endregion
}
