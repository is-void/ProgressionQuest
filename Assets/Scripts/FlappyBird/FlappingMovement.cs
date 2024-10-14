using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappingMovement : MovementSystem
{
    public LerpToTransform lerpScript;
    bool usePreciseMovement = false;
    bool upButton;
    bool jumpButton;
    bool downButton;
    bool AllowDownForce;
    public float flapForce = PlayerController.INITAL_JUMP_FORCE;
    public static int MaxFlappyJumpUpgrades = 3;
    public int gravUpgrade = 0;
    public float downForce = 2f;

    public float verticalSpeed = 4f;
    public float horizontalSpeed = 5f;

    private void Start()
    {
        gameOver = false;
        PlayerPrefs.SetInt("Gamemode", 1);
        PlayerPrefs.SetInt("Initialized", 1);
        OnSwitch(playerController);
        flapForce = PlayerPrefs.GetFloat("FlappyJumpForce");
        AllowDownForce = Toolbox.INT_TO_BOOL(PlayerPrefs.GetInt("FlappyDownControl"));
        usePreciseMovement = Toolbox.INT_TO_BOOL(PlayerPrefs.GetInt("Use Precise Movement"));
        playerController.score = (PlayerPrefs.GetInt("Starting Phase") - 1) * 20 ;
    }
    public new void OnSwitch(PlayerController playerController)
    {
        lerpScript = GetComponent<LerpToTransform>();
        lerpScript.enabled = false;
        Physics.gravity = Toolbox.FLOATS_TO_VECTOR3(0, PlayerPrefs.GetFloat("GravityYVal"), 0);
        base.OnSwitch(playerController);
        SetPlayer(playerController.gameObject);
        
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

    }

    public override void SystemFixedUpdate()
    {
        if(!gameOver)
        {
            CheckInput();
            MovePlayer();
        }
        
    }
    public override void SystemUpdate()
    {

        if(!gameOver)
        {
            CheckInput();
            MovePlayer();
        }
        

    }

    public override void Upgrade(string upgrade)
    {
        switch(upgrade)
        {
            case "Upgrade Jump" :
                UpgradeJump();
                break;

            case "Allow Down Control":
                UpgradeDownwardsControl();
                break;

            case "Upgrade Gravity Resistance":
                UpgradeGravResistance();
                break;

            case "Precise Movement":
                UpgradePreciseMovementControl();
                break;

            case "Start On Second Phase":
                StartOnSecondPhase();
                break;

            case "Start On Third Phase":
                StartOnThirdPhase();
                break;

            default:
                break;

        }
        
    }

    public void StartOnSecondPhase()
    {
        PlayerPrefs.SetInt("Starting Phase", 2);
    }
    public void StartOnThirdPhase()
    {
        PlayerPrefs.SetInt("Starting Phase", 3);
    }
    private void UpgradePreciseMovementControl()
    {
        usePreciseMovement = true;
        PlayerPrefs.SetInt("Use Precise Movement", Toolbox.BOOL_TO_INT(usePreciseMovement));
    }
    private void UpgradeDownwardsControl()
    {
        AllowDownForce = true;
        PlayerPrefs.SetInt("FlappyDownControl", Toolbox.BOOL_TO_INT(AllowDownForce));
        
    }
    private void UpgradeJump()
    {
        flapForce += 50;
        PlayerPrefs.SetFloat("FlappyJumpForce", flapForce);

    }

    private void UpgradeGravResistance()
    {
        PlayerPrefs.SetInt("GravUpgrades", PlayerPrefs.GetInt("GravUpgrades") + 1);
        PlayerPrefs.SetFloat("GravityYVal", -15F + 4 * PlayerPrefs.GetInt("GravUpgrades"));
        
    }

    private void MovePlayer()
    {
        UpdateMovement();
        downButton = false;
        upButton = false;
    }
    public void CheckInput()
    {

        jumpButton = Input.GetButtonDown("Jump");
        if(Input.GetAxis("Vertical") < 0)
            downButton = true;
        if (Input.GetAxis("Vertical") > 0)
            upButton = true;
    }
    
    public void UpdateMovement()
    {
        CheckInput();
        if (!usePreciseMovement)
        {
            if (jumpButton)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * flapForce);
            }

            if (downButton && AllowDownForce)
            {
                rb.AddForce(Vector3.down * downForce);
            }
        }
        else
        {
            rb.velocity /= 1.1f;
            int verticalInput = 0;
            
            if(upButton && !downButton)
            {
                verticalInput = 1;
            } else if(downButton & !upButton)
            {
                verticalInput = -1;
            }
            if(verticalInput != 0)
                rb.velocity = new Vector3(0, verticalInput * verticalSpeed, 0);

        }
    }
    public override void GameOver()
    {
        if(!gameOver)
        {
            Physics.gravity = new Vector3(0, -9.84f, 0);
            gameObject.transform.position -= Vector3.forward;
            gameOver = true;
            gameCollider.isTrigger = true;

        }
        else
        {
            player.GetComponent<PlayerController>().gameManager.gameState = GameManager.State.GAMEOVER;
            Debug.Log(player.GetComponent<PlayerController>().gameManager.gameState);
            
            
            PlayerPrefs.SetInt("FlapGame_HighScore", playerController.hiScore);
            PlayerPrefs.Save();
        }
        
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Pipe") || collision.gameObject.CompareTag("FinalObstacle"))
        {
            GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pipe"))
        {
            UpdateMoney();
        }
        if(other.gameObject.CompareTag("FinalObstacle"))
        {
            //PlayerPrefs.SetInt("FlappyLevel", PlayerPrefs.GetInt("FlappyLevel")+1);
            UpdateMoney();
            Victory();
        }

        if (other.gameObject.CompareTag("DeadZone"))
        {
            GameOver();
        }

        if (other.gameObject.CompareTag("Projectile"))
        {
            GameOver();
        }

    }

    private void UpdateMoney()
    {
        player.GetComponent<PlayerController>().score++;
        if (playerController.score > playerController.hiScore)
        {
            playerController.hiScore = playerController.score;
        }
        if (playerController.streak > PlayerPrefs.GetInt("Max Streak"))
            PlayerPrefs.SetInt("MaxStreak", playerController.streak);
        playerController.money += Mathf.FloorToInt(1f + Mathf.Pow(playerController.score + PlayerPrefs.GetInt("FlappyLevel") * 20f, 1.1f) + ((playerController.score + PlayerPrefs.GetInt("FlappyLevel") * 20f )/ 3));
        PlayerPrefs.SetInt("Money", playerController.money);
    }

    public override void Victory()
    {
        Debug.Log("END");
        CanvasGroup filter = GameObject.Find("Transition").GetComponentInChildren<CanvasGroup>();
        lerpScript.enabled = true;
        lerpScript.OnEnabled();
        StartCoroutine(UpdateFilter(filter, 0));

    }


    public IEnumerator UpdateFilter(CanvasGroup filter, int times)
    {
        yield return new WaitForFixedUpdate();
        times++;
        Debug.Log("Checks and Balances");
        filter.alpha = lerpScript.time;

        if (lerpScript.isMoving && times < lerpScript.totalFrames)
            StartCoroutine(UpdateFilter(filter, times));
        else
            playerController.gameManager.GoToNextScene();
    }
}
