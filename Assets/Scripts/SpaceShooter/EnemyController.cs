using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, EntityInterface
{
    [Header("Game")]
    public GameManager gameManager;

    [Header("Entity")]
    public HealthBar healthBar;
    public float health;
    public float maxHealth;

    [Header("Enemy")]
    public bool dontDespawn = false;
    public float damage;

    [Header("Loot")]
    public int score = 10;
    public int money = 100;
    public GameObject explosionParticle;
    public GameObject collisionParticle;

    [Header("Movement")]
    public EnemyLerpMovement[] movementPlan;
    public EnemyLerpMovement[] loopingMovementPlan;
    public EnemyLerpMovement currentMovement;
    public int planIndex;
    public bool shouldLoopMovement = false;
    private bool loopMovement = false;

    public int loopingIndex;
    
    

    // Start is called before the first frame update
    void Start()
    { 
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(!shouldLoopMovement)
            movementPlan = GetComponents<EnemyLerpMovement>();
        planIndex = 0;
        Debug.Log(movementPlan[planIndex]);
        currentMovement = movementPlan[planIndex];
        movementPlan[planIndex].enabled = true;
        movementPlan[planIndex].OnEnabled();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health <= 0)
        {
            OnDeath();
        }

        if(loopMovement)
        {
            if (currentMovement.doneMoving)
                NextLoopMovement();
            return;
        }
        
        if (planIndex < movementPlan.Length-1)
        {
            currentMovement = movementPlan[planIndex];
        }
        else
        {
                loopMovement = shouldLoopMovement;
        }
        if(currentMovement.doneMoving)
        {
            if(movementPlan.Length > planIndex+1)
                NextMovement();
        }

        if (health <= 0)
        {
            /*
                int index = Random.Range(0, powerups.Length);
                Instantiate(powerups[Random.Range(0, powerups.Length)], transform);
            */

            gameManager.playerController.UpdateScore(score);
            gameManager.playerController.AddMoney(money);
            Destroy(gameObject);
        }
            
        
    }

    public void NextLoopMovement()
    {
        currentMovement.doneMoving = false;
        currentMovement.enabled = false;
        currentMovement = loopingMovementPlan[loopingIndex];
        if(loopingIndex < loopingMovementPlan.Length-1)
        {
            loopingIndex++;
            
        } else
        {
            loopingIndex = 0;
        }
        currentMovement = loopingMovementPlan[loopingIndex];
        currentMovement.enabled = true;
        currentMovement.OnEnabled();

    }

    public void NextMovement()
    {
        currentMovement.doneMoving = false;
        currentMovement.enabled = false;
        planIndex++;
        currentMovement = movementPlan[planIndex];
        currentMovement.enabled = true;
        currentMovement.OnEnabled();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && !dontDespawn)
        {
            OnCollisionWithPlayer(1f);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.name == "BossHolder")
            OnCollisionWithPlayer(damage, 30f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Projectile"))
        {
            health -= other.gameObject.GetComponent<ProjectileInfo>().damage;
            Instantiate(collisionParticle, other.transform.position, other.transform.rotation);
            healthBar.UpdateBar();
            other.GetComponent<ProjectileInfo>().OnEnemyCollision();
        }

        if(other.gameObject.CompareTag("HurtZone") && !dontDespawn)
        {
            OnCollisionWithPlayer(2);

        }
    }

    public float getHealth()
    {
        return health;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public void OnDeath()
    {
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void OnCollisionWithPlayer(float dmgMulti)
    {
        ((SpaceShooterMovementSystem)(gameManager.playerController.movementSystem)).DealDamage(damage * dmgMulti);
        Destroy(gameObject);
    }

    public void OnCollisionWithPlayer(float dmgMulti, float dmgDivider)
    {
        ((SpaceShooterMovementSystem)(gameManager.playerController.movementSystem)).DealDamage((damage * dmgMulti)/dmgDivider);
    }
}
