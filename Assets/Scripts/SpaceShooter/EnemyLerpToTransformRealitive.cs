using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLerpToTransformRealitive : EnemyLerpMovement
{
    public Vector3 changeInPosition;
    public Vector3 changeInRotation;

    // Start is called before the first frame update
    
    // Rotation formula

    // x = (x - center) * sin(t)
    // y = (y - center) * sin(t)


    public override void EndTransform()
    {
        endPosition = initalPosition + changeInPosition;
        endRotation = Quaternion.Euler(initalRotation.eulerAngles + changeInRotation);
    }


    public override void LerpCalculations()
    {
        if(initalPosition != null && initalRotation != null && endPosition != null && time > 0)
        {
            transform.position = Vector3.Lerp(initalPosition, endPosition, time);
            //transform.rotation = Quaternion.Euler(Vector3.Lerp(initalRotation.eulerAngles, endRotation.eulerAngles, time));
        }
        
    }
}
