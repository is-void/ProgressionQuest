using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{

    public bool rbBased;
    public float scrollSpeedX = -10;
    public float scrollSpeedY = 0;
    public float scrollSpeedZ = 0;
    public int timeMulitplier;
    public float timeCalc;
    public Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.frameCount % 60 == 0)
            timeMulitplier++;

        if (rbBased)
            rb.velocity = new Vector3(scrollSpeedX, scrollSpeedY, scrollSpeedZ);
        else
            transform.Translate(new Vector3(scrollSpeedX, scrollSpeedY, scrollSpeedZ));
        

    }

    
}
