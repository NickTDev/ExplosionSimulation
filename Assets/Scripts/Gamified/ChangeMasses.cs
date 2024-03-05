using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMasses : MonoBehaviour
{
    Particle3D cannon;
    Particle3D ball;
    explosiveForce explosion;

    // Start is called before the first frame update
    void Start()
    {
        cannon = GameObject.Find("PhysicsCannon").GetComponent<Particle3D>();
        ball = GameObject.Find("CannonBall").GetComponent<Particle3D>();
        explosion = GameObject.Find("ExplosionGenerator").GetComponent<explosiveForce>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            cannon.mass += 1;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            cannon.mass -= 1;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ball.mass += 1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ball.mass -= 1;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            explosion.explosionStrength += 100;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            explosion.explosionStrength -= 100;
        }
    }
}
