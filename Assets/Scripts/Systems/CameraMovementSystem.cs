using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraMovementSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public Transform orientation;
    public Transform camTrans;

    public bool canRotate = true;
    
    [Header("Input")]
    public float sensX = 50f;
    public float sensY = 50f;
    public float mouseX;
    public float mouseY;

    
    public abstract void SwitchToSystem();
    public abstract void UpdatePosition();
    public abstract void GetInput();
}
