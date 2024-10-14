using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToTransform : MonoBehaviour
{

    public bool shouldUsePhysics = false;
    private Vector3 initalPosition;
    public Vector3 initalRotation;
    public GameObject endTransform;
    public int frames = 0;
    public int totalFrames = 240;
    public bool isMoving = false;
    public float time;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        OnEnabled();
    }
    public void OnEnabled()
    {
        isMoving = true;
        initalPosition = transform.position;
        frames = 0;
        time = 0;
        if (rb != null)
            rb.isKinematic = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        time = frames / (totalFrames + 0f);
        transform.rotation = Quaternion.Euler(Vector3.Lerp(initalRotation, endTransform.transform.rotation.eulerAngles, time));
        if(shouldUsePhysics)
        {
            if (transform.position != endTransform.transform.position)
                rb.velocity = (endTransform.transform.position - initalPosition)/(totalFrames);
        } else
        {
            transform.position = Vector3.Lerp(initalPosition, endTransform.transform.position, time);
        }
        
        isMoving = frames < totalFrames;
        if (isMoving)
        {
            frames++; 
        }
        else
        {
            if(rb != null)
                rb.isKinematic = false;
            enabled = false;
        }
            
    }
}
