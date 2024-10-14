using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject platform;
    public Vector3 spawnPoint;
    public LayerMask genericMask;
    public LayerMask groundMask;
    public LayerMask slipperyMask;
    public Vector3 platformBounds = Vector3.one;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckAvailibleSpawnSpot();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 3 || collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }

       
    }

    

    void CheckAvailibleSpawnSpot()
    {

        if (!(Physics.CheckBox(RoundVector(transform.position), platformBounds * 0.5f, Quaternion.identity, genericMask)  || Physics.CheckBox(RoundVector(transform.position), platformBounds * 0.5f, Quaternion.identity, groundMask) || Physics.CheckBox(RoundVector(transform.position), platformBounds * 0.5f, Quaternion.identity, slipperyMask)))
        {
            if (Vector3.Distance(GameObject.Find("Player").transform.position, RoundVector(transform.position)) > 1.5f)
                spawnPoint = RoundVector(transform.position);
            else
                spawnPoint = Vector3.zero;
             
        } else
        {
            Debug.Log("Bad SPAWN");
                
        }
            
        
        
    }
        
    Vector3 RoundVector(Vector3 vector)
    {
        return new Vector3(vector.x, Mathf.Round(vector.y * 4f)/4f, vector.z);
    }

    private void OnDestroy()
    {
        if(spawnPoint != null && spawnPoint != Vector3.zero)
            Instantiate(platform, spawnPoint, Quaternion.identity);
    }
}
