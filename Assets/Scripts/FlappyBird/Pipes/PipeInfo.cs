using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeInfo : MonoBehaviour
{
    public Vector3 spawnPos;
    public int minY, maxY;
    // Start is called before the first frame update
    void Start()
    {
        spawnPos = new Vector3(30, Random.Range(minY, maxY), 0);
        transform.position = spawnPos;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
