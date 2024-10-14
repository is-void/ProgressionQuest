using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Transform shootSpot;
    public GameObject projectile;
    public GameObject player;
    public Rigidbody rb;
    public Vector3 quedForce;
    public ForceMode mode;
    public Vector3 realitiveOffset;
    public GameObject cam;
    public bool canShoot;
    public float shotDelay;
    public bool bulletQued;

    public GameObject throwableItem;

    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    public void OnWeaponUpdate()
    {
        if(!canShoot)
            StartCoroutine(ShootingDelay(shotDelay));
    }

    private void FixedUpdate()
    {
        rb.AddForce(quedForce, mode);
        quedForce = Vector3.zero;
    }

    void LateUpdate()
    {
        transform.position = (cam.transform.position + cam.transform.right * realitiveOffset.x + cam.transform.up * realitiveOffset.y + cam.transform.forward * realitiveOffset.z);
        transform.rotation = cam.transform.rotation;
        if (bulletQued)
        {
            Instantiate(projectile, shootSpot.position, shootSpot.rotation);
        }
        bulletQued = false;
        
    }

    private void Update()
    {
        if(Input.GetMouseButton(0) && canShoot)
        {
            PrimaryFire();
            canShoot = false;
            StartCoroutine(ShootingDelay(shotDelay));
        }

        
    }
    public GameObject ThrowWeapon()
    {
       
        Debug.Log("Throw Weapon");
        GameObject item = Instantiate(throwableItem, cam.transform.position + Vector3.up + cam.transform.forward*1.5f, cam.transform.rotation);
        item.GetComponent<Rigidbody>().AddForce(cam.transform.forward.normalized * 100f);
        Debug.Log(item.transform.position);
        return item;
    }

    public abstract void PrimaryFire();
    public abstract void SecondaryFire();

    public IEnumerator ShootingDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canShoot = true;

    }

    
}
