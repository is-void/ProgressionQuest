using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMovement : MovementSystem
{
    

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float oldMoveSpeed = 6f;
    public float jumpForce = 10f;
    bool startEndSequence = false;
    public float momentuemMultiplier = 2f;
    public float groundMovementMultiplier = 10f;
    public float airMovementMultiplier = 0.4f;

    public ConstantForce floatForce;
    public float playerHeight = 2f;
    public bool holdingDown;
    float horizontalMovement;
    float verticalMovement;

    bool jumpQued;
    

    [Header("Collision Detection")]

    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask slipperyGroundMask;
    [SerializeField] LayerMask bouncyGroundMask;

    bool isGrounded;
    bool isSliding;
    bool isBouncing;
    float groundDrag = 6f;
    float oldGroundDrag = 6f;
    float airDrag = 0.0f;
    float groundDistance = 0.4f;
    

    [Header("Punch Ablity")]
    public bool isCharging;
    public bool isPunching;
    float punchDuration;
    float punchForceMulti;

    Vector3 moveDirection;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    public Vector3 slopeMoveDirection;
    RaycastHit slopeHit;


    [Header("Misc")]
    public Transform[] checkpoints;
    public int checkpointIndex;
    public float respawnTimer;
    bool isRespawning = false;
    public LerpToTransform playerEndLerp, playerSetLerp, platformLerp;
    public LerpToTransform startLerpScript;
    bool finalTransition = false;
    bool updateMovement = true;

    public override void Upgrade(string upgrade)
    {
        throw new global::System.NotImplementedException();
    }

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }

    private void Start()
    {
        OnSwitch(playerController);
        PlayerPrefs.SetInt("Gamemode", 3);
        transform.position = checkpoints[checkpointIndex].position + Vector3.up;
        transform.rotation = checkpoints[checkpointIndex].rotation;

    }

    IEnumerator Respawn()
    {
        canInput = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.AddTorque(0, 0, Random.Range(0, 30));

        yield return new WaitForSeconds(respawnTimer);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        transform.position = checkpoints[checkpointIndex].position + Vector3.up;
        transform.rotation = checkpoints[checkpointIndex].rotation;
        canInput = true;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        isRespawning = false;
    }
    public new void OnSwitch(PlayerController playerController)
    {
        base.OnSwitch(playerController);
        SetPlayer(playerController.gameObject);
        Physics.gravity = playerController.baseGravity;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        playerHeight = playerController.gameObject.GetComponentInChildren<CapsuleCollider>().height;
        checkpointIndex = PlayerPrefs.GetInt("Platformer_Checkpoint");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public override void SystemUpdate()
    {
        isSliding = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, slipperyGroundMask);
        isGrounded = !isSliding && Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask);
        isBouncing = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, bouncyGroundMask) && rb.velocity.y < 0;

        Debug.DrawRay(transform.position, transform.up * -1f, Color.red, playerHeight);
        SetDrag();
        GetInput();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

    }

    public override void SystemFixedUpdate()
    {
        if (!updateMovement)
            return;
        if (finalTransition && playerSetLerp.enabled == false)
        {
            finalTransition = false;
            playerEndLerp.enabled = true;
            playerEndLerp.OnEnabled();
            platformLerp.enabled = true;
            platformLerp.OnEnabled();
            Debug.Log("Starting end transition");
            CanvasGroup filter = GameObject.Find("Transition").GetComponentInChildren<CanvasGroup>();
            StartCoroutine(UpdateEndFilter(filter, 0));
        }
            
        MovePlayer();
        CheckMomentuem();
    }

    void CheckMomentuem()
    {

        if (Mathf.Abs(rb.velocity.x) > moveSpeed/1.2f)
        {
            Debug.Log("Vel > MoveSpd");
            if (isGrounded)
            {
                Debug.Log("Grounded Vel");
                moveSpeed += momentuemMultiplier * (0.1f / moveSpeed);
            }
            else
            {
                Debug.Log("!Grounded");
                if (rb.velocity.x / horizontalMovement > 0)
                    moveSpeed += momentuemMultiplier * (0.05f / moveSpeed);
                else
                {
                    Debug.Log("Reducing Speed");
                    moveSpeed -= 0.01f;
                }
                    
            }
                
        }
        else
        {
            Debug.Log("ResettingSPD");
            moveSpeed = oldMoveSpeed;

        }
            
    }

    void Jump()
    {
        jumpQued = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (isPunching)
        {
            if (isGrounded)
            {
                punchDuration = 10;
            }
            else
            {
                rb.AddForce(transform.up * jumpForce / 4f, ForceMode.Impulse);
                punchDuration = 10;
                return;
            }

        }
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void SetDrag()
    {
        if (isGrounded && !isSliding)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }

    public void GetInput()
    {
        if (!canInput)
            return;
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            jumpQued = true;
        }

        if (Input.GetKeyDown(jumpKey) && isPunching)
        {
            Jump();
        }

        if (isBouncing)
        {
            if (verticalMovement >= 0)
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * -0.95f, rb.velocity.z);
            else
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * -1.05f, rb.velocity.z);
        }


        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        holdingDown = (verticalMovement < 0);

        moveDirection = transform.right * horizontalMovement;

    }


    void MovePlayer()
    {
        float movementDivider = 1;
        if (isCharging)
            movementDivider = 2;

        if (isGrounded)
        {
            if (!OnSlope())
            {
                rb.AddForce(slopeMoveDirection.normalized * groundMovementMultiplier / movementDivider * moveSpeed, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * groundMovementMultiplier / movementDivider * moveSpeed, ForceMode.Acceleration);
            }
        }
        else if (isSliding)
        {
            if (!OnSlope())
            {
                rb.AddForce(moveDirection * airMovementMultiplier / movementDivider * moveSpeed, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(slopeMoveDirection * airMovementMultiplier / movementDivider * moveSpeed, ForceMode.Acceleration);
            }

        }
        else
        {
            rb.AddForce(moveDirection * airMovementMultiplier / movementDivider * moveSpeed, ForceMode.Acceleration);
        }

        if (jumpQued && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(jumpKey))
        {
            Debug.Log("Trying to jump, Grounded : " + isGrounded);
        }

        if (jumpQued && isGrounded)
        {
            Jump();
        }

    }

    
    public override void GameOver()
    {

    }

    public override void Victory()
    {
        throw new global::System.NotImplementedException();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("DeadZone") & !isRespawning)
        {
            isRespawning = true;
            StartCoroutine(Respawn());

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!updateMovement)
            return;

        if (other.gameObject.CompareTag("DeadZone") && !finalTransition && !isRespawning)
        {
            isRespawning = true;
            StartCoroutine(Respawn());

        }

        if (other.gameObject.CompareTag("Checkpoint"))
        {

            checkpointIndex = other.GetComponentInParent<Checkpoint>().checkpointIndex;
            PlayerPrefs.SetInt("Platformer_Checkpoint", checkpointIndex);
            other.GetComponentInParent<Checkpoint>().shouldActivate = false;
        }

        if (other.gameObject.CompareTag("BouncyGround"))
        {
            Debug.Log("Something");
            float yVel = rb.velocity.y;
            float multi = -0.98f;
            if (holdingDown)
                multi = -1.2f;
            rb.velocity = rb.velocity - Vector3.up * yVel;
            rb.velocity += Vector3.up * yVel * multi;



        }
        if (other.gameObject.CompareTag("BouncyWall"))
        {
            Debug.Log("SomethingElse");
            float multi = -1f;
            float xVel = rb.velocity.x;
            rb.velocity -= Vector3.right * xVel;
            rb.velocity += Vector3.right * xVel * multi;
            Debug.Log("Hit wall");
        }

        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("FinalLocation") && !isRespawning && isGrounded && !startEndSequence)
        {
            startEndSequence = true;
            
            EndTransition();
        }
    }

    public void InitialTransition()
    {

        if (checkpointIndex == 0)
        {
            PlayerPrefs.SetInt("Initialized", 3);
            startLerpScript = GetComponent<LerpToTransform>();
            CanvasGroup filter = GameObject.Find("Transition").GetComponentInChildren<CanvasGroup>();
            startLerpScript.enabled = true;
            startLerpScript.OnEnabled();
            StartCoroutine(UpdateFilter(filter, 0));

        }
        else
        {
            Physics.gravity = playerController.baseGravity;
            updateMovement = true;
        }

    }
    public IEnumerator UpdateFilter(CanvasGroup filter, int times)
    {
        yield return new WaitForFixedUpdate();
        times++;
        filter.alpha = 1f - startLerpScript.time;

        if (startLerpScript.isMoving && times < startLerpScript.totalFrames)
            StartCoroutine(UpdateFilter(filter, times));
        else
        {
            updateMovement = true;
            Physics.gravity = playerController.baseGravity;
            yield break;

        }
    }

    public void EndTransition()
    {
        finalTransition = true;
        rb.isKinematic = true;
        playerSetLerp.enabled = true;
        playerSetLerp.OnEnabled();

    }
    public IEnumerator UpdateEndFilter(CanvasGroup filter, int times)
    {
        yield return new WaitForFixedUpdate();
        Debug.Log("SOMETHIGG");
        times++;
        filter.alpha = playerEndLerp.time;

        if (times < playerEndLerp.totalFrames)
            StartCoroutine(UpdateEndFilter(filter, times));
        else
        {
            
            playerController.gameManager.GoToNextScene();

        }
    }
}

