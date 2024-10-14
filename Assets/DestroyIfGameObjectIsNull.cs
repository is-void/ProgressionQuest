using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfGameObjectIsNull : MonoBehaviour
{
    public GameObject obj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (obj == null)
            Destroy(gameObject);
    }
}
