using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class increaseAccel : MonoBehaviour
{
    //GameObject shot = GameObject.Find("swoopShot");
    //Particle2D particle = shot.GetComponent<Particle2D>();
    Particle2D particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        particle.acceleration.y += 0.7f;
    }
}
