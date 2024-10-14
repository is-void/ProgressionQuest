using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbox : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static Vector3 FLOATS_TO_VECTOR3(float x, float y, float z)
    {
        return new Vector3(x, y, z);
    }

    public static bool INT_TO_BOOL(int x)
    {
        if (x == 1)
            return true;
        return false;

    }

    public static int BOOL_TO_INT(bool x)
    {
        if(x)
            return 1;
        return 0;

    }

    public static int ABSCEIL_TO_INT(float f)
    {
        if(IS_POSITIVE(f))
            return Mathf.CeilToInt(f);
        return Mathf.FloorToInt(f);
    }

    public static bool IS_POSITIVE(float f)
    {
        return f >= 0;
    }
}
