using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotInfo : MonoBehaviour
{
    public bool canShoot = true;
    public float shotCooldown;
    // Start is called before the first frame update
    public void Shoot()
    {
        canShoot = false;
        StartCoroutine(Cooldown());
    }
    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(shotCooldown);
        canShoot = true;

    }


}
