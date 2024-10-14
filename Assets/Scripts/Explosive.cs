using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public Rigidbody rigidbod;
    public GameObject explosionParticle;
    public float explosionRadius = 2f;
    public float power = 10f;
    public float health = 50f;

    private void Update()
    {
        if (rigidbod.velocity.magnitude > 50f)
            Explode();
    }

    // Update is called once per frame

    public void Explode()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider col in cols)
        {
            if (col.CompareTag("Explosive"))
                col.GetComponent<Explosive>().Explode(0.5f);
            if (col.transform.root.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Debug.Log(rb);
                Vector3 forceDirection = -1f * (gameObject.transform.position - col.gameObject.transform.position).normalized;
                rb.AddForce(forceDirection * power);

            }


        }

        Instantiate(explosionParticle, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    public void Explode(float delay)
    {
        StartCoroutine(WaitToExplode(delay));

    }

    IEnumerator WaitToExplode(float delay)
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        if(collision.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("PlayerHit");
            Debug.Log("Velocity = " + collision.gameObject.GetComponentInParent<Rigidbody>().velocity.magnitude);
            if (collision.collider.gameObject.GetComponentInParent<Rigidbody>().velocity.magnitude > 3)
            {
                
                Explode();
            }
                
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile"))
            Explode();

    }

    private void OnDestroy()
    {
        
    }
}
