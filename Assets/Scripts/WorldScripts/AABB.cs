using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB : MonoBehaviour
{
    Vector3 center;
    float xRadius;
    float yRadius;
    float zRadius;

    private void Start()
    {
        center = transform.position;
        xRadius = transform.localScale.x / 2;
        yRadius = transform.localScale.y / 2;
        zRadius = transform.localScale.z / 2;
    }

    public Vector3 getClosestPoint(Particle3D particle)
    {
        Vector3 q = Vector3.zero;

        //Min of x/y/z is center.x/y/z - x/y/zRadius
        //Max of x/y/z is center.x/y/z + z/y/zRadius

        //Calculate closest x
        float temp = particle.transform.position.x;
        if (temp < (center.x - xRadius))
            temp = center.x - xRadius;
        if (temp > (center.x + xRadius))
            temp = center.x + xRadius;
        q.x = temp;

        //Calculate closest y
        temp = particle.transform.position.y;
        if (temp < (center.y - yRadius))
            temp = center.y - yRadius;
        if (temp > (center.y + yRadius))
            temp = center.y + yRadius;
        q.y = temp;

        //Calculate closest z
        temp = particle.transform.position.z;
        if (temp < (center.z - zRadius))
            temp = center.z - zRadius;
        if (temp > (center.z + zRadius))
            temp = center.z + zRadius;
        q.z = temp;

        return q;
    }

    public bool testSphereCollision(Particle3D particle)
    {
        Vector3 closestPoint = getClosestPoint(particle);
        Vector3 temp = closestPoint - particle.transform.position;

        if (Vector3.Dot(temp, temp) <= ((particle.transform.localScale.x / 2) * (particle.transform.localScale.x / 2)))
            return true;

        return false;
    }
}
