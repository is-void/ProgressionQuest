using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public GameObject effectProjectiles;
    public Vector3 force;   

    public override void PrimaryFire()
    {
        float forceMulti = -1f;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, 2f))
            forceMulti = -1.5f;
        mode = ForceMode.Impulse;
        quedForce =  forceMulti *( cam.transform.right * force.x + cam.transform.up * force.y + cam.transform.forward * force.z);
        bulletQued = true;

    }

    public override void SecondaryFire()
    {
        
    }

}
