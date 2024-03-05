using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Integrator3D
{
    public static void integrate(ref Vector3 pos, ref Vector3 velocity, Vector3 accel, Vector3 accum, float damp, float invMass)
    {
        pos = pos + (velocity * Time.deltaTime);
        
        Vector3 resultingAccel = accel;
        resultingAccel += accum * invMass;

        velocity = (velocity * (Mathf.Pow(damp, Time.deltaTime))) + (resultingAccel * Time.deltaTime);
    }
}
