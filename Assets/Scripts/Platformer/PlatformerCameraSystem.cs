using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCameraSystem : CameraMovementSystem
{
    public GameObject cameraSpot;

    float xRotation;
    float yRotation;


    private void Update()
    {
        UpdatePosition();
    }

    public override void UpdatePosition()
    {
        GetInput();
        camTrans.position = cameraSpot.transform.position;
    
    }
    public override void SwitchToSystem()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameObject.SetActive(true);

    }



    public override void GetInput()
    {
       
    }
}
