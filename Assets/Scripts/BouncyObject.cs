using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyObject : MonoBehaviour
{
    public float perserveVelocityIndex = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        col.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(col.gameObject.GetComponent<Rigidbody>().velocity.x, -col.gameObject.GetComponent<Rigidbody>().velocity.y * perserveVelocityIndex, col.gameObject.GetComponent<Rigidbody>().velocity.z);
    }
}
