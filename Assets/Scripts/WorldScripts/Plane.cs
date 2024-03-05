using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    Vector3 normal;
    float offset;

    public Vector3 getNormal()
    {
        normal = transform.up;
        return normal;
    }

    public float getOffset()
    {
        offset = Vector3.Dot(normal, transform.position);
        return offset;
    }
}
