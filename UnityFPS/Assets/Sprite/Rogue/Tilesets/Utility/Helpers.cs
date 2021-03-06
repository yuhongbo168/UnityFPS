using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public static class TransformExtension
{
    public static Bounds InverseTransformBounds(this Transform transform, Bounds worldBounds)
    {
        var center = transform.InverseTransformPoint(worldBounds.center);

        var extents = worldBounds.extents;

        var axisX = transform.InverseTransformVector(extents.x, 0, 0);
        var axisY = transform.InverseTransformVector(0, extents.y, 0);
        var axisZ = transform.InverseTransformVector(0, 0, extents.z);

        extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
        extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
        extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

        return new Bounds { center = center, extents = extents };
    }
}


