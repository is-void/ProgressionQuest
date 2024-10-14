using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    // Start is called before the first frame update

    public bool hasMinXBound;
    public bool hasMaxXBound;
    public bool hasMinYBound;
    public bool hasMaxYBound;
    public bool hasMinZBound;
    public bool hasMaxZBound;

    public int minXBound;
    public int maxXBound;
    public int minYBound;
    public int maxYBound;
    public int minZBound;
    public int maxZBound;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position.x < minXBound && hasMinXBound) || (transform.position.x > maxXBound && hasMaxXBound))
        {
            Destroy(gameObject);
        }

        if ((transform.position.y < minYBound && hasMinYBound) || (transform.position.y > maxYBound && hasMaxYBound))
        {
            Destroy(gameObject);
        }

        if ((transform.position.z < minZBound && hasMinZBound) || (transform.position.z > maxZBound && hasMaxZBound))
        {
            Destroy(gameObject);
        }
    }
}
