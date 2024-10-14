using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyLerpMovement : MonoBehaviour
{
    public EnemyController enemyController;
    [HideInInspector] public Vector3 initalPosition;
    [HideInInspector] public Quaternion initalRotation;

    [HideInInspector] public Vector3 endPosition;
    [HideInInspector] public Quaternion endRotation;
    [HideInInspector] public GameObject endTransform;

    [HideInInspector] public int frames = 0;
    public int seconds = 1;
    public int totalFrames = 1;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool doneMoving = false;
    [HideInInspector] public float time;

    public void OnEnabled()
    {
        frames = 0;
        time = 0;
        totalFrames = 60 * seconds;
        isMoving = true;
        initalPosition = transform.position;
        initalRotation = transform.rotation;
        EndTransform();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time = frames / (totalFrames + 0f);
        LerpCalculations();
        isMoving = frames < totalFrames;
        if (isMoving)
        {
            frames++;
        }
        else
        {
            doneMoving = true;
            
        }

    }
    public virtual void EndTransform()
    {
        if (endTransform != null)
        {
            endPosition = endTransform.transform.position;
            endRotation = endTransform.transform.rotation;
        }
        
    }



    public abstract void LerpCalculations();

}
