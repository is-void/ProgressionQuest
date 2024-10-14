using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject projectile;
    public GameObject shootSpot;
    public bool shouldShoot = true;
    public float minShotsPerSecond;
    public float maxShotsPerSecond;
    public float initalDelay;

    void Start()
    {
        StartCoroutine(ShootingRoutine(initalDelay));  
    }

    
    // Update is called once per frame

    void ShootProjectile()
    {     
        Instantiate(projectile, shootSpot.transform);

    }

    IEnumerator ShootingRoutine(float intialDelay)
    {
        yield return new WaitForSeconds(initalDelay);
        while (shouldShoot)
        {
            if (minShotsPerSecond == 0)
                yield return new WaitForSeconds(1);
            else
                yield return new WaitForSeconds(Random.Range((1f / minShotsPerSecond), (1f / maxShotsPerSecond)));
            Debug.Log("SHOT");
            if (Random.Range(1, 10) % 6 == 0)
            {
                yield return new WaitForSeconds(4);
            }
            ShootProjectile();
        }

    }

    

}
