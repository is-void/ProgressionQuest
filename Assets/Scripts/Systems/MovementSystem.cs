using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementSystem : MonoBehaviour
{
    public Rigidbody rb;
    public Collider gameCollider;
    public GameObject player;
    public PlayerController playerController;
    public GameObject cameraHolder;
    public Transform orientation;
    public bool canInput = true;
    public bool gameOver = false;
    public enum System
    {
        FLAPPY,
        SPACESHOOTER,
        FPS
    }
    public struct Bounding
    {
        public float leftBound;
        public float rightBound;
        public float topBound;
        public float bottomBound;
    }

    public System system;

    
    public void OnSwitch(PlayerController playerController)
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetPlayer(GameObject p)
    {
        player = p;
        rb = player.GetComponent<Rigidbody>();

    }

    public abstract void Victory();
    public abstract void Upgrade(string upgrade);

    public abstract void SystemUpdate();
    public abstract void SystemFixedUpdate();

    public abstract void GameOver();


}
