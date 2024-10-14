using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMoveSystem : MovementSystem
{
    public Quaternion weaponAssignRotation;
    public Vector3 weaponAssignPos;
    public GameObject cam;
    public GameObject currentWeapon;
    public GameObject[] weapons;
    public int weaponIndex;
    public bool isUsingGauntlet;
    [Header("Movement")]
    
    public float moveSpeed = 6f;
    public float jumpForce = 10f;
    
    public float groundMovementMultiplier = 10f;
    public float airMovementMultiplier = 0.4f;

    public ConstantForce floatForce;
    float playerHeight = 2f;
    float horizontalMovement;
    float verticalMovement;
    
    bool jumpQued;

    [Header ("Collision Detection")]

    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask slipperyGroundMask;
    [SerializeField] LayerMask bouncyGroundMask;


    bool canJump = true;
    bool isGrounded;
    bool isSliding;
    bool isBouncing;
    float groundDrag = 6f;
    float airDrag = 0.1f;
    float oldGroundDrag = 6f;
    float groundDistance = 0.4f;

    [Header("Checkpoints")]
    public Transform[] checkpoints;
    public bool isRespawning;
    public int checkpointIndex;
    public int respawnTimer;



    [Header("Punch Ablity")]
    public bool isCharging;
    public bool isPunching;
    float punchDuration;
    float punchForceMulti;
    
    Vector3 moveDirection;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    Vector3 slopeMoveDirection;
    RaycastHit slopeHit;

    LerpToTransform lerpScript;


    public void InitialTransition()
    {
        CanvasGroup filter = GameObject.Find("Transition").GetComponentInChildren<CanvasGroup>();
        if (PlayerPrefs.GetInt("Initialized") < 4)
        {
            StartCoroutine(UpdateFilter(filter, 0, 120));
            PlayerPrefs.SetInt("Initialized", 4);
        }
        else
        {
            transform.position = checkpoints[checkpointIndex].position;
        }
    }
    public IEnumerator UpdateFilter(CanvasGroup filter, int times, int maxTimes)
    {
        yield return new WaitForFixedUpdate();
        times++;
        filter.alpha = 1 - (float)times/maxTimes;

        if (times < maxTimes)
            StartCoroutine(UpdateFilter(filter, times, maxTimes));
        else
        {
            yield break;

        }
    }

    public override void Upgrade(string upgrade)
    {
        throw new global::System.NotImplementedException();
    }

    bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight/2 + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }

    private void Start()
    {
        OnSwitch(playerController);
        PlayerPrefs.SetInt("Gamemode", 4);
        InitialTransition();

    }
    public new void OnSwitch(PlayerController playerController)
    {
        base.OnSwitch(playerController);
        SetPlayer(playerController.gameObject);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        Physics.gravity = playerController.baseGravity * 1.5f;
        canJump = true;
        checkpointIndex = PlayerPrefs.GetInt("FPS_Checkpoint");
        cam = cameraHolder.transform.GetChild(0).gameObject;
        playerHeight = playerController.gameObject.GetComponentInChildren<CapsuleCollider>().height;
        UpdateWeapon();

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

        CheckSwapWeapons();

        if(isRespawning)
        {
            Weapon weapon;
            if(currentWeapon != null && currentWeapon.TryGetComponent<Weapon>(out weapon))
            {
                weapon.GetComponent<Weapon>().canShoot = false;
            }
                
        }

    }

    void CheckSwapWeapons()
    {
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject thrownWeapon = null;
            if(weaponIndex != 0)
            {
                thrownWeapon = currentWeapon.GetComponent<Weapon>().ThrowWeapon();
                thrownWeapon.GetComponent<ResetToIntialPosition>().SetValues(weaponAssignPos, weaponAssignRotation);
                weaponIndex = 0;
                UpdateWeapon();
                
            }
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2f) && hit.collider.isTrigger && hit.collider.transform.parent.gameObject != thrownWeapon && hit.collider.CompareTag("Pickup"))
            {
                Debug.Log(hit);
                weaponIndex = hit.collider.gameObject.GetComponent<PickupInfo>().index;
                weaponAssignPos = hit.collider.transform.parent.gameObject.GetComponent<ResetToIntialPosition>().position;
                weaponAssignRotation = hit.collider.transform.parent.GetComponent<ResetToIntialPosition>().rotation;
                Destroy(hit.collider.transform.parent.gameObject);
                UpdateWeapon();
            }
        }
        

    }

    public override void SystemFixedUpdate()
    {
        MovePlayer();
    }

    void Jump()
    {
        if(canJump)
        {
            jumpQued = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (isUsingGauntlet)
            {
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

            }
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        

    }

    void SetDrag()
    {
        if (isGrounded || isPunching)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }
   
    public void GetInput()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            jumpQued = true;
        }

        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;


        if (isUsingGauntlet)
        {
            if (Input.GetKeyDown(jumpKey) && isPunching)
            {
                Jump();
            }
            if (Input.GetMouseButtonDown(1) && !isPunching)
            {
                Debug.Log("Charging");
                punchForceMulti = 0;
                punchDuration = 10;
            }
            if (Input.GetMouseButton(1) && !isPunching)
            {
                if (punchDuration <= 40f)
                {
                    isCharging = true;
                    punchForceMulti += 0.3f;
                    punchDuration += 1f / 3f;
                }
                else
                {
                    punchDuration = 40f;
                    isCharging = false;
                }



            }
            if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("Released");
                isCharging = false;
            }
        }
    }
    
    void UpdateWeapon()
    {
        currentWeapon = weapons[weaponIndex];
        currentWeapon.SetActive(true);
        foreach(GameObject w in weapons)
        {
            if(w != currentWeapon)
            {
                w.SetActive(false);
            }
        }
        if(weaponIndex != 0)
            currentWeapon.GetComponent<Weapon>().OnWeaponUpdate();
    }

    private void MovePlayerWhilePunching()
    {
        if(!isCharging && Mathf.Floor(punchDuration) > 0)
        {
            floatForce.enabled = true;
            if (OnSlope())
            {
                rb.AddForce(Vector3.ProjectOnPlane(orientation.forward, slopeHit.normal).normalized * punchForceMulti * moveSpeed, ForceMode.Acceleration);
                
            }
            else
            {
                rb.AddForce(orientation.forward * punchForceMulti * moveSpeed, ForceMode.Acceleration);
            }
            punchDuration--;
            isPunching = true;

        } else
        {
            isPunching = false;
            floatForce.enabled = false;
        }
            

    }

    void MovePlayer()
    {
        float movementDivider = 1;
        if(isCharging)
            movementDivider = 2;
        if(isUsingGauntlet)
        {
            MovePlayerWhilePunching();
            if (isPunching)
                return;
        }
        
        
        if(isBouncing)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * (rb.velocity.y + 2)/rb.velocity.y, rb.velocity.z);
        }
        if(isGrounded)
        {
            if(!OnSlope())
            {
                rb.AddForce(slopeMoveDirection.normalized * groundMovementMultiplier / movementDivider * moveSpeed, ForceMode.Acceleration);
            } else
            {
                rb.AddForce(moveDirection.normalized * groundMovementMultiplier / movementDivider * moveSpeed, ForceMode.Acceleration);
            }
        }
        else if(isSliding)
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

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeadZone") & !isRespawning)
        {
            isRespawning = true;
            StartCoroutine(Respawn());

        }

        if (other.gameObject.CompareTag("Checkpoint"))
        {
            Debug.Log("Something");
            checkpointIndex = other.GetComponentInParent<Checkpoint>().checkpointIndex;
            PlayerPrefs.SetInt("FPS_Checkpoint", checkpointIndex);
            other.GetComponentInParent<Checkpoint>().shouldActivate = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeadZone") & !isRespawning)
        {
            isRespawning = true;
            StartCoroutine(Respawn());

        }
    }

    IEnumerator Respawn()
    {
        canJump = false;
        canInput = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.AddTorque(0, 0, Random.Range(0, 30));

        yield return new WaitForSeconds(respawnTimer);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        transform.position = checkpoints[checkpointIndex].position + Vector3.up;
        transform.rotation = checkpoints[checkpointIndex].rotation;
        canInput = true;
        canJump = true;
        UpdateWeapon();
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        isRespawning = false;
    }


    public override void GameOver()
    {
        
    }

    public override void Victory()
    {
        throw new global::System.NotImplementedException();
    }
}
