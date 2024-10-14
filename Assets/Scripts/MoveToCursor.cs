using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCursor : MonoBehaviour
{
    public RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.position = Input.mousePosition;
    }
}
