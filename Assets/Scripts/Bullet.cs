using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Collider col;
    public Rigidbody rb;
    public float speed;
    public GameObject bulletParticles;
    void Start()
    {
        Instantiate(bulletParticles, transform.position, transform.rotation);
        rb.velocity = transform.forward * speed;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Physics.IgnoreCollision(col, collision.collider);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Physics.IgnoreCollision(col, collision.collider);
    }
}
