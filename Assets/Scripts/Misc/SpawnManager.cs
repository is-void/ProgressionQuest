using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public SpawnBehaviour spawnBehaviour;
    public bool shouldSpawn = true;
    void Start()
    {
        SetSpawning(spawnBehaviour);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void SetSpawning(SpawnBehaviour behaviour)
    {
        StopCoroutine(SpawnObstacle());

        spawnBehaviour = behaviour;
        shouldSpawn = true;
        StartCoroutine(Delay(spawnBehaviour.startDelay));
        StartCoroutine(SpawnObstacle());
    }

    void SpawnMethod()
    {
        spawnBehaviour.GeneratePos();
        spawnBehaviour.Spawn();
    }

    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
    IEnumerator SpawnObstacle()
    {
       
        while (shouldSpawn)
        {
            yield return new WaitForSeconds(spawnBehaviour.spawnInterval);
            SpawnMethod();
        }
        

    }
    
}
