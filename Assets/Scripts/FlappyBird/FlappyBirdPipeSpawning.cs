using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdPipeSpawning : SpawnBehaviour
{
    public GameManager game;
    public GameObject firstPipePrefab;
    public GameObject secondPipePrefab;
    public GameObject finalPipe;
    public GameObject currentPipePrefab;
    public float firstSpawnInterval, secondSpawnInterval, thirdSpawnInterval;
    public int score;
    public int pipeToSpawn;
    public int levelInterval = 20;
    private GameObject last;
    public GameObject[] pipePrefabs;

    void Start()
    {
        spawnInterval = firstSpawnInterval;
        startDelay = 5f;
        //score = game.playerController.score.flappyScore;
        score = 0;
        pipeToSpawn = (PlayerPrefs.GetInt("Starting Phase") - 1) * 20;
        CheckSpawnInterval();
    }
    public override void GeneratePos()
    {
        
    }

    public override bool shouldSpawn()
    {
        return true;
    }

    public override void Spawn()
    {
        if (pipeToSpawn < 60)
        {
            spawnPos = new Vector3(30, Random.Range(-2, 5), 0);

            CheckSpawnInterval();
            PipeToSpawn();
            last = Instantiate(currentPipePrefab, spawnPos, currentPipePrefab.transform.rotation);

        } else if(pipeToSpawn == 60)
        {
            Instantiate(finalPipe, spawnPos, finalPipe.transform.rotation);
        }
        pipeToSpawn++;
        
    }

    public void PipeToSpawn()
    {
        Debug.Log(pipeToSpawn + " and " + spawnInterval);
        currentPipePrefab = pipePrefabs[pipeToSpawn / levelInterval];
    }

    private void Update()
    {
        Debug.Log(PlayerPrefs.GetInt("Starting Phase"));
    }

    public void CheckSpawnInterval()
    {
        switch (pipeToSpawn)
        {
            case 0:
                spawnInterval = firstSpawnInterval;
                break;
            case 20:
                spawnInterval = secondSpawnInterval;
                break;

            case 40:
                spawnInterval = thirdSpawnInterval;
                break;
        }
    }
}
