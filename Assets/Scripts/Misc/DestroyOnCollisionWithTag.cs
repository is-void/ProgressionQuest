using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollisionWithTag : MonoBehaviour
{
    public bool destroySelf;
    public bool destroyOther;
    public string otherTag;
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
        if (collision.gameObject.CompareTag(otherTag))
        {
            if(destroyOther)
                Destroy(collision.gameObject);
            if(destroySelf)
                Destroy(this);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(otherTag))
        {
            if (destroyOther)
                Destroy(collision.gameObject);
            if (destroySelf)
                Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(otherTag))
        {
            
            if (destroyOther)
                Destroy(other.gameObject);
            if (destroySelf)
                Destroy(this);
        }
    }
}
