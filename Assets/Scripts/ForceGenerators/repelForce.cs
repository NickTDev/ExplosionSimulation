using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class repelForce : ParticleForceGenerator
{
    Vector2 attrForce;
    bool isPressed;

    void Update()
    {
        isPressed = Input.GetMouseButton(1);
    }

    public override void updateForce(Particle2D particle)
    {
        if (isPressed)
        {
            Debug.Log("Mouse Clicked");
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = worldPos - particle.transform.position;
            float k = 1000.0f;
            attrForce = (direction.normalized * k) / (direction.SqrMagnitude());
        }
        else
        {
            attrForce = Vector2.zero;
        }

        if (attrForce.sqrMagnitude > 0.001)
        {
            attrForce *= -1;
            particle.addForce(attrForce);
        }
    }

    public override void updateForce(Particle3D particle)
    {
        if (isPressed)
        {
            Debug.Log("Mouse Clicked");
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = worldPos - particle.transform.position;
            float k = 1000.0f;
            attrForce = (direction.normalized * k) / (direction.SqrMagnitude());
        }
        else
        {
            attrForce = Vector2.zero;
        }

        if (attrForce.sqrMagnitude > 0.001)
        {
            attrForce *= -1;
            particle.addForce(attrForce);
        }
    }
}
