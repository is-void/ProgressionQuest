using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallActivation : MonoBehaviour
{
    public LerpToTransform lerp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Activate()
    {
        lerp.enabled = true;
    }
}
