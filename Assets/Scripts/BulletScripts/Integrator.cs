using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Integrator
{
    public static void integrate(ref Vector2 pos, ref Vector2 velocity, Vector2 accel, Vector2 accum, float damp, float invMass)
    {
        pos = pos + (velocity * Time.deltaTime);

        Vector2 resultingAccel = accel;
        resultingAccel += accum * invMass;

        velocity = (velocity * (Mathf.Pow(damp, Time.deltaTime))) + (resultingAccel * Time.deltaTime);
    }
}
