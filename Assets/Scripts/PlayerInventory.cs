using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Powerup> powerups;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void AddPowerUp(GameObject powup)
    {
        Debug.Log("ADDING A SUPER COOL POWERUP :)");
        Powerup p = powup.GetComponent<Powerup>();
        p.SetTarget(gameObject);
        p.OnReady();
        powerups.Add(p);
    }
}
