using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInfo : MonoBehaviour
{
    public Scroll movement;
    public bool hasSpread = false;
    public float xVarience;
    public bool dontDestroy = false;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        if(hasSpread)
        {
            movement.scrollSpeedX += Random.Range(-xVarience, xVarience);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnemyCollision()
    {
        if(!dontDestroy)
            Destroy(gameObject);
    }

    public void OnPlayerCollision()
    {
        if (!dontDestroy)
            Destroy(gameObject);
    }
}
