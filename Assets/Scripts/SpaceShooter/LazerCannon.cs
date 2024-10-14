using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerCannon : MonoBehaviour
{
    public float idleTime;
    public float prepTime;
    public float lazerTime;
    public GameObject lazer;
    public GameObject lazerLight;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChargeLazer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChargeLazer()
    {
        lazer.SetActive(false);
        lazerLight.SetActive(false);
        yield return new WaitForSeconds(idleTime);
        lazer.SetActive(false);
        lazerLight.SetActive(true);
        StartCoroutine(ShootLazer());
       

    }

    IEnumerator ShootLazer()
    {
        yield return new WaitForSeconds(prepTime);
        lazer.SetActive(true);
        lazerLight.SetActive(true);
        StartCoroutine(ResetLazer());
    }

    IEnumerator ResetLazer()
    {
        yield return new WaitForSeconds(lazerTime);
        StartCoroutine(ChargeLazer());
    }
}
