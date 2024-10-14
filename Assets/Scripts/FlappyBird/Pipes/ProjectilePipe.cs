using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePipe : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject projectile;
    public GameObject[] shootingLocations;
    public bool shouldShoot = true;
    GameObject leftEdge;
    public float minShotsPerSecond;
    public float maxShotsPerSecond;

    void Start()
    {
        leftEdge = GameObject.Find("Left Edge of Screen");
        StartCoroutine(ShootingRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShootProjectile()
    {
        int randomIndex = Random.Range(0, shootingLocations.Length);
        if(transform.position.x < leftEdge.transform.position.x + 19)
            Instantiate(projectile, shootingLocations[randomIndex].transform);


    }
    
    IEnumerator ShootingRoutine()
    {
        while(shouldShoot)
        {
            yield return new WaitForSeconds(Random.Range((1f/minShotsPerSecond), (1f/maxShotsPerSecond)));
            if(Random.Range(1, 10) % 6 == 0)
            {
                yield return new WaitForSeconds(4);
            }
            ShootProjectile();
        }

    }


}
