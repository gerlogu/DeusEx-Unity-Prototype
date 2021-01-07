using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Movement --> Walk, Run, Jump, Hookshot
/// </summary>
public class PlayerMovementOld : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 gravityVelocity;
    [SerializeField] private Transform groundChecker;
    private Transform ceilingChecker;
    private bool shutDownLantern = true;
    private bool touchingCeiling = false;
    private CameraMovement cameraMovement;

    [Header("Player Variables")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float ceilingDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask sliceableWallMask;
    [SerializeField] private LayerMask hookMask;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float maxDashTime = 0.1f;
    [SerializeField] private float dashSpeed = 1000.0f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private int maxDashes = 2;
    [SerializeField] private float accelerationFactor;

    private Inclination inclination;
    public enum Inclination
    {
        NONE,
        LEFT,
        RIGHT
    }

    [Header("Crouch")]

    private bool m_Crouch = false;
    private float m_OriginalHeight;
    [SerializeField] private float m_CrouchHeight = 0.5f;


    [Header("Others")]
    [SerializeField] private GameObject lantern;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float lightIntensity;
    private GameManager gameManager;
    
    private float currentFrontMov;
    private float currentLateralMov;
    private int numberOfJumps;
    private bool sprinting;

    public delegate void InitSprint();
    public delegate void EndSprint();
    public delegate void StartDash();

    public InitSprint OnInitSprint;
    public EndSprint OnEndSprint;
    public StartDash OnDash;

    public float movementQuantity;
    private bool isDashing = false;

    private float currentDashTime;
    private int currentDashes;

    private Transform rightChecker;
    private Transform leftChecker;
    private bool affectedByGravity = true;
    private bool leftWall;
    private bool rightWall;

    private bool grounded;
    private SoundManager soundManager;
    private AudioSource audioSource;

    private Headbob headbob;

    private float m_CrouchDistance;

    private void Awake()
    {
        hookshotTransform.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        // Cursor.visible = false;
    }

     public float fov;
    private void Start()
    {
        #region Init
        if (!gameManager)
            gameManager = FindObjectOfType<GameManager>();
        headbob = GetComponent<Headbob>();
        cameraMovement = GetComponentInChildren<CameraMovement>();
        controller = GetComponent<CharacterController>();
        groundChecker = GameObject.FindGameObjectWithTag("GroundChecker").transform;
        ceilingChecker = GameObject.FindGameObjectWithTag("CeilingChecker").transform;
        soundManager = FindObjectOfType<SoundManager>();
        audioSource = GetComponent<AudioSource>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        fov = cam.GetComponent<CameraFOVController>().fieldOfView;
        #endregion

        // DEPRECATED FOR CROUCH
        m_OriginalHeight = controller.height;
        m_CrouchDistance = (m_OriginalHeight - m_CrouchHeight) / 2;

        #region Movement Variables
        maxJumps = maxJumps - 1;
        numberOfJumps = maxJumps;
        currentDashes = maxDashes;
        #endregion

        OnInitSprint += () => {
            sprinting = true;
        };
        OnEndSprint += () => {
            sprinting = false;
        };

        currentDashTime = maxDashTime;

        rightChecker = GameObject.FindGameObjectWithTag("RightChecker").transform;
        leftChecker = GameObject.FindGameObjectWithTag("LeftChecker").transform;
    }

    void StopSpeedParticles()
    {
        speedParticles.enableEmission = false;
    }

    

    private GameObject lastHookObject;

    RaycastHit outlineHookHit;

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (canMove && gameManager.playerState == PlayerState.IN_GAME)
        {
            hookshotTransform.LookAt(hookshotPosition);

            #region  Player state
            switch (state)
            {
                case State.Normal:
                    Movement();
                    CheckSliceableWall();
                    if (hookshotTransform.localScale.z <= 0.1f)
                        HandleHookshotStart();
                    hookshotSize = Mathf.Lerp(hookshotSize, 0, 20f*Time.deltaTime);
                    hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);
                    break;
                case State.HookshotThrown:
                    HandleHookshotThrow();
                    Movement();
                    CheckSliceableWall();
                    break;
                case State.HookshotFlyingPlayer:
                    HandleHookshotMovement();
                    break;

                case State.MissedHook:
                    if (timeForHookshot > 0)
                    {
                        timeForHookshot -= Time.deltaTime;
                    }
                    else
                    {
                        state = State.Normal;
                    }
                    Movement();
                    CheckSliceableWall();
                    break;
                default:
                    Debug.LogError("Player state not supported.");
                    break;
            }
            #endregion
        }

        if (FindObjectOfType<WeaponSway>())
        {
            if (!FindObjectOfType<WeaponSway>().enabled)
                FindObjectOfType<WeaponSway>().enabled = (gameManager.playerState == PlayerState.IN_GAME) ? true : false;
        }

        if (!cameraMovement.enabled)
            cameraMovement.enabled = (gameManager.playerState == PlayerState.IN_GAME) ? true : false;

        if (!IsGrounded())
        {
            onAir = true;
            canReproduceSteps = false;
        }

        if (IsGrounded() && onAir)
        {
            onAir = false;
            Invoke("SetCanReproduceToTrue", .44f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            inclination = inclination == Inclination.RIGHT ? Inclination.NONE : Inclination.RIGHT;
            print(inclination);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            inclination = inclination == Inclination.LEFT ? Inclination.NONE : Inclination.LEFT;
        }
    }

    bool slowMotion = false;

    void SetCanReproduceToTrue()
    {
        if(!onAir)
            canReproduceSteps = true;
    }

    #region Hook
    private Transform cam;
    [Header("Hook")]
    [SerializeField] private float hookMovementSpeedMultiplier = 3f;
    [SerializeField] private Transform hookshotTransform;
    [SerializeField] private float endHookMaxTimer = 15f;
    [SerializeField] private float momentum = 1f;
    [SerializeField] private float momentumDrag = 2f;
    [SerializeField] private ParticleSystem speedParticles;
    
    public State state = State.Normal;

    [Header("Hook Debug")]
    private Vector3 characterVelocityMomentum;
    private Transform debugHitpoint;

    private Vector3 hookshotPosition;
    private Vector3 hookshotDirection;
    private float hookMovementSpeed;
    private float hookshotSize;
    private float endHookTimer;

    public enum State
    {
        Normal,
        HookshotThrown,
        HookshotFlyingPlayer,
        MissedHook
    }

    RaycastHit hookRaycastHit;
    /// <summary>
    /// Checks if player has pressed the key to fire the hook.
    /// </summary>
    private void HandleHookshotStart()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(cam.position, cam.forward, out hookRaycastHit, 45, hookMask))
            {
                if (debugHitpoint)
                    debugHitpoint.position = hookRaycastHit.point;

                hookshotPosition = hookRaycastHit.point;
                hookshotSize = 0f;
                hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);
                hookshotTransform.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;

                if (soundManager && audioSource)
                {
                    soundManager.player.PlaySFX(audioSource, 4);
                }
                
                state = State.HookshotThrown;
            }
            else
            {
                state = State.MissedHook;
            }
        }
    }

    private float timeForHookshot;

    /// <summary>
    /// Handles Hookshot Throw.
    /// </summary>
    private void HandleHookshotThrow()
    {
        float hookshotThrowSpeed = 90f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);


            if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
            {
                endHookTimer = endHookMaxTimer;
                if (hookRaycastHit.collider.gameObject.layer == 19)
                {
                    numberOfJumps = maxJumps;
                    currentDashes = maxDashes;
                    state = State.HookshotFlyingPlayer;
                    gravityVelocity.y = 0;
                    move = Vector3.zero;
                    
                    if (IsGrounded())
                    {
                        Jump(jumpHeight / 1.75f);
                    }
                }
                else
                {
                    state = State.Normal;
                }
            }
            
    }

    /// <summary>
    /// Handles Hookshot Movement.
    /// </summary>
    private void HandleHookshotMovement()
    {
        
        hookMovementSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), 10f, 20f);

        if (gravityVelocity.y <= 3)
        {
            endHookTimer -= Time.deltaTime;

            if (endHookTimer <= endHookMaxTimer / 1.1f)
            {
                speedParticles.enableEmission = true;
            }

            if (endHookTimer <= endHookMaxTimer / 1.2f)
            {
                hookshotSize = Mathf.Lerp(hookshotSize, 0, 20f * Time.deltaTime);
                hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);
            }

            Movement();
            gravityVelocity.y = 0;

            if (endHookTimer <= 0)
            {
                cam.GetComponent<CameraFOVController>().fieldOfView = fov;
                Invoke("StopSpeedParticles", .15f);
                state = State.Normal;
                InitMomentum(momentum);
            }

            // Move character controller
            hookshotDirection = (hookRaycastHit.point - cam.position).normalized;
            controller.Move(hookshotDirection * hookMovementSpeed * hookMovementSpeedMultiplier * Time.deltaTime);
            cam.GetComponent<CameraFOVController>().fieldOfView = fov + 7f;
            if (CancelHook(KeyCode.Space))
            {
                state = State.Normal;
                InitMomentum(momentum);
                Jump(jumpHeight);
                numberOfJumps++;
                Invoke("StopSpeedParticles", .1f);
            }

            if (/*CancelHook(KeyCode.E) ||*/ IsGrounded() || Physics.Raycast(this.transform.position, (this.hookshotPosition - this.transform.position).normalized, 7.5f, hookMask))
            {
                cam.GetComponent<CameraFOVController>().fieldOfView = fov;
                gravityVelocity.y = 0;
                state = State.Normal;
                Invoke("StopSpeedParticles", 0);
                InitMomentum(momentum);
            }


        }
        else
        {
            gravityVelocity.y += gravity * Time.deltaTime;
            controller.Move(gravityVelocity * Time.deltaTime);
        }

        touchingCeiling = Physics.CheckSphere(ceilingChecker.position, ceilingDistance, groundMask);
        if (touchingCeiling && gravityVelocity.y > 0)
        {
            gravityVelocity.y = -0.25f;
        }

        float reachedHookshotPositionDistance = 1f;

        if ((Vector3.Distance(cam.position, hookshotPosition) < reachedHookshotPositionDistance) || (IsGrounded() && gravityVelocity.y == 0))
        {
            cam.GetComponent<CameraFOVController>().fieldOfView = fov;
            state = State.Normal;
            Invoke("StopSpeedParticles", 0);
        }

    }

    private void InitMomentum(float momentumExtraSpeed)
    {
        characterVelocityMomentum = new Vector3(hookshotDirection.x, hookshotDirection.y, hookshotDirection.z) * hookMovementSpeed * momentumExtraSpeed;
    }

    private bool CancelHook(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            return true;
        }
        return false;
    }
    #endregion
    private bool canReproduceSteps = false;

    /// <summary>
    /// Checks lateral walls to slice.
    /// </summary>
    private void CheckSliceableWall()
    {
        // Does the ray intersect any objects excluding the player layer
        if (Physics.CheckSphere(rightChecker.position, 0.4f, sliceableWallMask)
            && !IsGrounded())
        {
            if (!rightWall)
            {
                rightWall = true;
                numberOfJumps = maxJumps;
            }
        }
        else
        {
            if (rightWall)
            {
                rightWall = false;
                currentDashes = maxDashes;
            }
        }

        // Does the ray intersect any objects excluding the player layer
        if (Physics.CheckSphere(leftChecker.position, 0.4f, sliceableWallMask) && !IsGrounded())
        {
            if (!leftWall)
            {
                leftWall = true;

                numberOfJumps = maxJumps;
            }
        }
        else
        {
            if (leftWall)
            {
                leftWall = false;
                currentDashes = maxDashes;
            }
        }
    }



    #region Movement Functions
    /// <summary>
    /// Checks dash state and updates it.
    /// </summary>
    /// <param name="direction_x">Player movement x</param>
    /// <param name="direction_z">Player movement y</param>
    /// <returns></returns>
    private float CalculateDashState(float direction_x, float direction_z)
    {

        float dash;
        if (Input.GetMouseButtonDown(3) && !isDashing && !IsGrounded() && move != Vector3.zero && currentDashes > 0 && !leftWall && !rightWall)
        {
            movement = transform.right * direction_x + transform.forward * direction_z;
            currentDashTime = 0.0f;
            soundManager.player.PlaySFX(audioSource, 3);
            isDashing = true;
            OnDash();
            currentDashes--;
            // Debug.Log("Dash");
        }

        if (currentDashTime < maxDashTime)
        {
            dash = dashSpeed;
            isDashing = true;
            currentDashTime += Time.deltaTime;
        }
        else
        {
            movement = Vector3.zero;
            isDashing = false;
            dash = 0;
        }

        return dash;
    }

    /// <summary>
    /// Resets movement variable to default.
    /// </summary>
    private void ResetMovementVariables()
    {
        numberOfJumps = maxJumps;
        currentDashes = maxDashes;
    }

    /// <summary>
    /// If player is not grounded, impact sound against floor can be played.
    /// </summary>
    public void SetCanImpactSound()
    {
        if (!grounded)
            canImpactSound = true;
    }

    /// <summary>
    /// Player jump.
    /// </summary>
    /// <param name="jumpHeight">Jump height quantity</param>
    private void Jump(float jumpHeight)
    {
        gravityVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        if (soundManager && audioSource && !IsGrounded())
        {
            numberOfJumps--;
            soundManager.player.PlaySFX(audioSource, 1);
        }
    }

    /// <summary>
    /// Checks if player is touching the floor.
    /// </summary>
    /// <returns>Bool that determines if player is touching or not the floor</returns>
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
    }

    /// <summary>
    /// Set canSoundStep to true.
    /// </summary>
    private void SetCanSoundStepToTrue()
    {
        canSoundStep = true;
    }
    #endregion


    #region Movement
    private bool canImpactSound = false;
    private bool canSoundStep = true;

    private Vector3 movement;         // Movement vector
    private Vector3 move;             // Move direction
    private float left_movement = 0;  // Left direction movement quantity
    private float right_movement = 0; // Right direction movement quantity
    private float front_movement = 0; // Front direction movement quantity
    private float back_movement = 0;  // Back direction movement quantity

    private float headbobIntensity = 0.00125f; // Headbob intensity
    private bool onAir = false;                // Bool that determines if player is on air

    /// <summary>
    /// Player Movement.
    /// </summary>
    private void Movement()
    {
        #region Crouch
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (!m_Crouch)
            {
                m_Crouch = true;
                print("Crouch");
                controller.height = m_CrouchHeight;
                groundDistance -= m_CrouchHeight;
                transform.position = new Vector3(transform.position.x, transform.position.y - m_CrouchHeight, transform.position.z);
            }
            else
            {
                m_Crouch = false;
                print("Uncrouch");
                controller.height = m_OriginalHeight;
                groundDistance += m_CrouchHeight;
                transform.position = new Vector3(transform.position.x, transform.position.y + m_CrouchHeight, transform.position.z);
            }
            
            
        }
        #endregion

        #region Touching Ceiling Checking
        touchingCeiling = Physics.CheckSphere(ceilingChecker.position, ceilingDistance, groundMask);
        
        if (IsGrounded() && gravityVelocity.y < 0)
        {
            gravityVelocity.y = -2f;
        }
        #endregion

        #region Movement AWSD
        Vector3 myInput = new Vector3(0, 0, 0);
        move = new Vector3();
        float left_direction = 0;
        float right_direction = 0;
        float front_direction = 0;
        float back_direction = 0;

        // Vector3 myInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Input.GetKey(KeyCode.S) && state != State.HookshotFlyingPlayer)
        {
            back_direction = 1;
        }
        else
        {
            back_direction = 0;
        }

        if (Input.GetKey(KeyCode.W))
        {
            front_direction = 1;
        }
        else
        {
            front_direction = 0;
        }

        currentFrontMov = front_direction-back_direction;
        myInput.z = currentFrontMov;

        if (Input.GetKey(KeyCode.D) )
        {
            right_direction = 1;
        }
        else
        {
            right_direction = 0;
        }
        if (Input.GetKey(KeyCode.A) )
        {
            left_direction = 1;
        }
        else
        {
            left_direction = 0;
        }

        currentLateralMov = right_direction - left_direction;
        myInput.x = currentLateralMov;

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (lantern)
            {
                if (shutDownLantern)
                {
                    lantern.GetComponent<Light>().intensity = lightIntensity;
                    shutDownLantern = false;
                }

                else
                {
                    lantern.GetComponent<Light>().intensity = 0;
                    shutDownLantern = true;
                }
            }


        }

        myInput.Normalize();
        
        float direction_x = myInput.x;
        float direction_z = myInput.z;

        float factor = accelerationFactor * Time.deltaTime;
        // Prueba

        // Vector3 myInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Input.GetKey(KeyCode.S) && state != State.HookshotFlyingPlayer)
        {
            back_movement += factor;

            back_movement = Mathf.Clamp(back_movement, 0.0f, -myInput.z);
            // print("Back: " + back);
            // print("-myInput.z = " + -myInput.z);
        }
        else
        {
            back_movement = 0;
        }

        if (Input.GetKey(KeyCode.W))
        {
            front_movement += factor;
            front_movement = Mathf.Clamp(front_movement, 0.0f, myInput.z);
        }
        else
        {
            front_movement = 0;
        }

        currentFrontMov = front_movement - back_movement;
        myInput.z = currentFrontMov;

        if (Input.GetKey(KeyCode.D) )
        {
            right_movement += factor;
            right_movement = Mathf.Clamp(right_movement, 0.0f, myInput.x);
        }
        else
        {
            right_movement = 0;
        }
        if (Input.GetKey(KeyCode.A) )
        {
            left_movement += factor;
            left_movement = Mathf.Clamp(left_movement, 0.0f, -myInput.x);
        }
        else
        {
            left_movement = 0;
        }

        currentLateralMov = right_movement - left_movement;
        myInput.x = currentLateralMov;

        // myInput.Normalize();
        // Fin Prueba
        #endregion

        #region Calculate movementQuantity
        movementQuantity = myInput.magnitude; // Movement Quantity (For Debugging)

        float x = myInput.x; // Lateral Movement
        float z = myInput.z; // Forward Movement

        move = transform.right * x + transform.forward * z; // Movement Calculation (Horizontal)
        #endregion

        #region Sprint Checking
        float sprintVelocity = 1f;

        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded() && !sprinting && myInput != Vector3.zero)
        {
            OnInitSprint(); // Delegate Call
        }

        if ((Input.GetKeyUp(KeyCode.LeftShift) || !IsGrounded() || myInput == Vector3.zero) && sprinting )
        {
            OnEndSprint(); // Delegate Call
        }

        if (sprinting)
        {
            sprintVelocity = 1.2f; // New Velocity
            cam.GetComponent<CameraFOVController>().fieldOfView = fov + 5f; // Speed Effect
        }
        else
        {
            sprintVelocity = 1f; // Normal Velocity
            cam.GetComponent<CameraFOVController>().fieldOfView = fov; // Normal Field Of View
        }
        #endregion

        #region Horizontal Movement Final Calculation & Assignation
        if(move != Vector3.zero && IsGrounded())
        {
            SoundEmitter.SpawnSoundSphere(transform.position, 10);
        }
        controller.Move(move * speed * sprintVelocity * Time.deltaTime);
        #endregion

        float dash = CalculateDashState(direction_x, direction_z);

        controller.Move(movement * dash * Time.deltaTime);

        if (touchingCeiling && gravityVelocity.y > 0)
        {
            gravityVelocity.y = -1.5f;
            print("ceiling");
        }

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) && numberOfJumps > 0)
        {
            if (!sprinting)
            {
                Jump(jumpHeight);
            }
            else
            {
                Jump(jumpHeight * 1.5f);
            }
        }
        

        if((!affectedByGravity || rightWall || leftWall) && gravityVelocity.y < 0)
        {
            gravityVelocity.y = !(isDashing || (IsGrounded() && gravityVelocity.y < 0)) ? (gravity / 110) : 0;
        }
        else
        {
            if (!IsGrounded())
            {
                if ((direction_x != 0 || direction_z != 0) && characterVelocityMomentum.magnitude < 10f)
                {
                    characterVelocityMomentum = Vector3.zero;
                }
                else
                {
                    // Apply Hook Momentum
                    Vector3 velocityHook = new Vector3();
                    velocityHook += characterVelocityMomentum;
                    controller.Move(velocityHook * Time.deltaTime);
                }
                
            }

            // Gravity
            gravityVelocity.y = !(isDashing || (IsGrounded() && gravityVelocity.y < 0)) ? gravityVelocity.y + gravity * Time.deltaTime : 0;
        }

        controller.Move(gravityVelocity * Time.deltaTime);

        #region Headbob Movement & Step Sounds
        if (IsGrounded() && (direction_x != 0 || direction_z != 0) && !sprinting)
        {
            headbobIntensity = Mathf.Lerp(headbobIntensity, 0.00125f, 1 * Time.deltaTime);
            headbob.HeadbobMovement(headbobIntensity, headbobIntensity, 5.75f);
            if (headbob.weaponHolder.transform.localPosition.y < -0.151 && canSoundStep && canReproduceSteps)
            {
                soundManager.player.PlaySFX(audioSource, 0);
                canSoundStep = false;
                Invoke("SetCanSoundStepToTrue", 0.5f);
            }
        }
        else if (IsGrounded() && (direction_x != 0 || direction_z != 0) && sprinting)
        {
            headbobIntensity = Mathf.Lerp(headbobIntensity, 0.00125f, 1 * Time.deltaTime);
            headbob.HeadbobMovement(headbobIntensity, headbobIntensity, 7.5f);
            if (headbob.weaponHolder.transform.localPosition.y < -0.151 && canSoundStep && canReproduceSteps)
            {
                soundManager.player.PlaySFX(audioSource, 0);
                canSoundStep = false;
                Invoke("SetCanSoundStepToTrue", 0.4f);
            }
        }
        else if(IsGrounded())
        {
            headbobIntensity = Mathf.Lerp(headbobIntensity, 0.00125f, 1 * Time.deltaTime);
            headbob.HeadbobMovement(headbobIntensity, headbobIntensity, 1f);
        }
        #endregion

        #region Camera Angle
        cameraMovement.rightWall = rightWall || inclination.Equals(Inclination.LEFT) ? true : false;
        cameraMovement.leftWall = leftWall || inclination.Equals(Inclination.RIGHT) ? true : false;
        #endregion

        #region Check Grounded Sound
        if (IsGrounded())
        {
            if (!grounded)
            {
                grounded = true;
                if (soundManager && audioSource && canImpactSound)
                {
                    soundManager.player.PlaySFX(audioSource, 2);
                }
                canImpactSound = false;
            }
        }
        else
        {
            if (grounded)
            {
                grounded = false;
                Invoke("SetCanImpactSound", 0.3f);
            }
        }
        #endregion

        // Movement Variables reset
        if (IsGrounded())
        {
            ResetMovementVariables();
        }

        // Momentum Damped
        if(characterVelocityMomentum.magnitude > 0)
        {
            //float momentumDrag = 6f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if(characterVelocityMomentum.magnitude < .4f || IsGrounded())
            {
                characterVelocityMomentum = Vector3.zero;
            }
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundChecker.position, groundDistance);
    }
}
