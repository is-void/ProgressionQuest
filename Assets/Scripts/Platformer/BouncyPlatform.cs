using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Check");
            collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(collision.gameObject.GetComponent<Rigidbody>().velocity.x, collision.gameObject.GetComponent<Rigidbody>().velocity.y * -1.2f, collision.gameObject.GetComponent<Rigidbody>().velocity.z);
        }
    }
}
