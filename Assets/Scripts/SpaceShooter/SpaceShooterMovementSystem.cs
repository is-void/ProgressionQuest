using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShooterMovementSystem : MovementSystem, EntityInterface
{
    [Header("Entity")]
    public float health = 10;
    public float maxHealth = 10;
    public HealthBar healthBar;

    [Header("Game")]
    public GameManager gameManager;
    public Bounding playerBounds;
    public SpaceShooterSpawning shooterSpawning;

    [Header("Shooting")]
    public GameObject shootSpot;
    public GameObject projectilePrefab;
    public int weaponIndex;
    public GameObject[] weapons;
    public ShotInfo[] weaponInfo;
    public int timeSinceLastShot;
    public int reloadTime;


    [Header("Input")]
    public int horizontalInput;
    public int horizonalMovementSpeed;
    public bool canAct = false;


    [Header("Misc")]
    public LerpToTransform lerpScript;
    public LerpToTransform endLerpScirpt;
    public bool continousHealthUpdates;

    private void Start()
    {

        Debug.Log(PlayerPrefs.GetInt("SPACESHOOTER_WeaponIndex"));
        weaponIndex = PlayerPrefs.GetInt("SPACESHOOTER_WeaponIndex");
        PlayerPrefs.SetInt("Gamemode", 2);
        
        maxHealth = 10 + PlayerPrefs.GetFloat("SPACESHOOTER_HealthUpgrades") * 10;
        shootSpot = GameObject.Find("Shoot Spot");

        playerBounds.leftBound = GameObject.Find("Left Edge of Screen").transform.position.x;
        playerBounds.rightBound = GameObject.Find("Right Edge of Screen").transform.position.x;
        projectilePrefab = weapons[weaponIndex];
        
        health = maxHealth;
        
        Physics.gravity = Vector3.zero;
        
        OnSwitch();
    }

    public void OnSwitch()
    {
        base.OnSwitch(playerController);
        SetPlayer(playerController.gameObject);
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        InitialTransition();



    }


    public void InitialTransition()
    {
        lerpScript = GetComponent<LerpToTransform>();
        CanvasGroup filter = GameObject.Find("Transition").GetComponentInChildren<CanvasGroup>();
        lerpScript.enabled = true;
        lerpScript.OnEnabled();
        if(PlayerPrefs.GetInt("Initialized") < 2)
        {
            StartCoroutine(UpdateFilter(filter, 0));
            PlayerPrefs.SetInt("Initialized", 2);
        }
        else
        {
            shooterSpawning.BeginPhase();
            canAct = true;
        }
    }
    public IEnumerator UpdateFilter(CanvasGroup filter, int times)
    {
        yield return new WaitForFixedUpdate();
        times++;
        filter.alpha = 1f - lerpScript.time;

        if (lerpScript.isMoving && times < lerpScript.totalFrames)
            StartCoroutine(UpdateFilter(filter, times));
        else
        {
            canAct = true;
            shooterSpawning.enabled = true;
            shooterSpawning.BeginPhase();
            yield break;

        }
    }


    public override void GameOver()
    {
        gameOver = true;
        gameManager.gameState = GameManager.State.GAMEOVER;
    }

    public override void SystemFixedUpdate()
    {

    }

    public override void Upgrade(string upgrade)
    {
        switch (upgrade)
        {
            case "BuyHealth":
                BuyHealth();
                break;
            case "BuyTriShot":
                PurchaseTriShot();
                break;
            case "BuyGattling":
                BuyGattling();
                break;
            default :
                break;
        }
    }

    
    public void BuyHealth()
    {
        PlayerPrefs.SetFloat("SPACESHOOTER_HealthUpgrades", PlayerPrefs.GetFloat("SPACESHOOTER_HealthUpgrades") + 1);
    }

    public void PurchaseTriShot()
    {
        PlayerPrefs.SetInt("SPACESHOOTER_WeaponIndex", 1);
    }

    public void BuyGattling()
    {
        PlayerPrefs.SetInt("SPACESHOOTER_WeaponIndex", 2);
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        healthBar.UpdateBar();

    }
    public float getHealth()
    {
        return health;
    }
    public float getMaxHealth()
    {
        return maxHealth;
    }
    public override void SystemUpdate()
    {
        if (!gameOver)
        {
            CheckHealth();
            CheckInput();
            MovePlayer();
        }

    }

    void CheckHealth()
    {
        if (continousHealthUpdates)
        {
            healthBar.UpdateBar();
        }
        if (health <= 0)
        {
            GameOver();
        }
    }

    public void MovePlayer()
    {
        UpdateMovement();
        CheckInBounds();
    }
    public void UpdateMovement()
    {

        rb.velocity = transform.right * horizontalInput * horizonalMovementSpeed;
    }

    public void CheckInput()
    {
        if (canAct)
        {
            horizontalInput = Toolbox.ABSCEIL_TO_INT(Input.GetAxisRaw("Horizontal"));
            if (Input.GetKey(KeyCode.Space) && weaponInfo[weaponIndex].canShoot)
            {
                Shoot();
                weaponInfo[weaponIndex].Shoot();
            }
            
        }
        else
        {
            horizontalInput = 0;
        }
        reloadTime--;

    }

    public void CheckInBounds()
    {
        if (transform.position.x <= playerBounds.leftBound && rb.velocity.x < 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }

        if (transform.position.x >= playerBounds.rightBound && rb.velocity.x > 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }
    }

    public void Shoot()
    {
        Instantiate(projectilePrefab, shootSpot.transform.position, projectilePrefab.transform.rotation);
    }

    public override void Victory()
    {
        canAct = canInput = false;
        Debug.Log("END");
        CanvasGroup filter = GameObject.Find("Transition").GetComponentInChildren<CanvasGroup>();
        filter.alpha = 0;
        endLerpScirpt.enabled = true;
        endLerpScirpt.OnEnabled();
        StartCoroutine(UpdateEndFilter(filter, 0));

    }


    public IEnumerator UpdateEndFilter(CanvasGroup filter, int times)
    {
        yield return new WaitForFixedUpdate();
        times++;
        Debug.Log("Checks and Balances");
        filter.alpha = endLerpScirpt.time;

        if (endLerpScirpt.isMoving && times < endLerpScirpt.totalFrames)
            StartCoroutine(UpdateEndFilter(filter, times));
        else
            playerController.gameManager.GoToNextScene();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EnemyProjectile"))
        {
            DealDamage(other.gameObject.GetComponent<ProjectileInfo>().damage);
            other.gameObject.GetComponent<ProjectileInfo>().OnPlayerCollision();
        }
    }
}


