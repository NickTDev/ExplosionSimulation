using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB : MonoBehaviour
{
    Vector3 center;
    Vector3[] orientation = new Vector3[3];
    float[] halfWidths = new float[3];

    private void Start()
    {
        center = transform.position;

        orientation[0] = transform.right;
        orientation[1] = transform.up;
        orientation[2] = transform.forward;

        halfWidths[0] = transform.localScale.x / 2;
        halfWidths[1] = transform.localScale.y / 2;
        halfWidths[2] = transform.localScale.z / 2;
    }

    public Vector3 getClosestPoint(Particle3D particle)
    {
        Vector3 d = particle.transform.position - center;
        Vector3 q = center;

        for (int i = 0; i < 3; i++)
        {
            float dist = Vector3.Dot(d, orientation[i]);

            if (dist > halfWidths[i])
                dist = halfWidths[i];
            if (dist < -halfWidths[i])
                dist = -halfWidths[i];

            q += dist * orientation[i];
        }

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
