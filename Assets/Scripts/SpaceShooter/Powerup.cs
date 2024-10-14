using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{

    public GameObject target;
    
    public int powerUp;
    public int duration;

    public abstract void OnReady();
    public void SetTarget(GameObject target)
    {
        this.target = target;
        if(duration != -1)
            StartCoroutine(PowerUpDuration());
    }
    IEnumerator PowerUpDuration()
    {
        yield return new WaitForSeconds(duration);
        EndOfDuration();


    }

    public abstract void EndOfDuration();

}
