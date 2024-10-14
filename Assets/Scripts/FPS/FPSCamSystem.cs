using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamSystem : CameraMovementSystem
{
    public GameObject cameraSpot;
    float multiplier = 0.01f;
    
    float xRotation;
    float yRotation;


    private void Update()
    {
        UpdatePosition();
    }

    public override void UpdatePosition()
    {
        GetInput();

        camTrans.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        camTrans.position = cameraSpot.transform.position;

        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }
    public override void SwitchToSystem()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameObject.SetActive(true);
        
    }

    

    public override void GetInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
