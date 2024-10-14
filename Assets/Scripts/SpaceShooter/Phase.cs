using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase : MonoBehaviour
{
    public int enemiesLeft;
    public int startingEnemies;

    // Start is called before the first frame update
    void Start()
    {
        startingEnemies = gameObject.transform.childCount;
        enemiesLeft = startingEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        enemiesLeft = gameObject.transform.childCount;
    }
}
