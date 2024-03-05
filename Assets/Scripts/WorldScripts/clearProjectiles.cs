using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearProjectiles : MonoBehaviour
{
    Particle3D[] particles;
    private void Update()
    {
        particles = FindObjectsOfType<Particle3D>();

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Pressed C");
            for (int i = 0; i < particles.Length; i++)
            {
                Debug.Log("Cleared");
                Destroy(particles[i].gameObject);
            }
        }
    }
}
