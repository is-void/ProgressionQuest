using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetToIntialPosition : MonoBehaviour
{
    public Vector3 position;
    public Quaternion rotation;
    public bool isOriginal;
    // Start is called before the first frame update
    private void Start()
    {
        if(isOriginal)
        {
            position = transform.position;
            rotation = transform.rotation;
        }
            
    }

    // Update is called once per frame
    public void SetValues(Vector3 pos, Quaternion rot)
    {
        
        position = pos;
        rotation = rot;
    }
    void ResetTranform()
    {
        transform.position = position;
        transform.rotation = rotation;
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("DeadZone"))
        {
            ResetTranform();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            ResetTranform();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            ResetTranform();
        }
    }
}
