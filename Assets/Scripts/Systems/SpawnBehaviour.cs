using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnBehaviour : MonoBehaviour
{
    public Vector3 spawnPos;
    public float spawnInterval;
    public float startDelay;

    public abstract void Spawn();
    public abstract void GeneratePos();

    public abstract bool shouldSpawn();
}
