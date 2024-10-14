using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGun : Powerup
{
    public float radius;
    public Rigidbody targetRb;
    public GameObject pellet;
    public GameObject gunRender;
    public Vector3 shootPoint;
    public GunPointIdentitfier gunPointIdentifier;
    public LayerMask groundLayer;
    public GameObject pellete;
    public float shotPower;
    bool ready = false;
    float angle;

    public override void OnReady()
    {
        duration = -1;
        gunPointIdentifier = target.GetComponentInChildren<GunPointIdentitfier>();
        targetRb = target.GetComponent<Rigidbody>();
        transform.position = gunPointIdentifier.gameObject.transform.position;
        ready = true;

    }
    // Update is called once per frame
    void Update()
    {
        if (!ready)
            return;

        
        angle =  Mathf.Atan2((Input.mousePosition.y - Screen.height / 2) , (Input.mousePosition.x - Screen.width / 2));

        
        gunRender.transform.position = gunPointIdentifier.gameObject.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    

    void Shoot()
    {
        if(pellete == null)
        {
            Vector3 launchDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;
            Debug.Log(angle * 90f);
            pellete = Instantiate(pellet, gunRender.transform.position - launchDirection/3f, Quaternion.identity);
            pellete.GetComponent<Rigidbody>().velocity = launchDirection * shotPower;
        }
        
              

    }
    public override void EndOfDuration()
    {
        
    }
}
