using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public CameraMovementSystem system;
    public Transform cam;


    void Start()
    {
        system.SwitchToSystem();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cam.position;
        transform.rotation = cam.rotation;
    }
}
