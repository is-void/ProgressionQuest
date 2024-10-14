using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public GameObject effectProjectiles;
    public Vector3 force;
    public float groundMulti = -1.2f;
    

    public override void PrimaryFire()
    {
        float forceMulti = -1f;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, 2f))
            forceMulti = groundMulti;
        mode = ForceMode.Impulse;
        quedForce =  forceMulti *( cam.transform.right * force.x + cam.transform.up * force.y + cam.transform.forward * force.z);
        bulletQued = true;
       
    }

    public override void SecondaryFire()
    {
        
    }

}

