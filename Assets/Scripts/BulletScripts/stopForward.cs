using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopForward : MonoBehaviour
{
    float timeLeft = 1;
    bool hasStopped;
    Particle2D particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
        hasStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else if (timeLeft <= 0 && !hasStopped)
        {
            Vector2 newVector = Vector2.zero;
            particle.setVelocity(newVector);
            particle.acceleration.y -= 5.0f;
            hasStopped = true;
        }
    }
}
