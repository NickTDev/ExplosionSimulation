using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class displayData : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cannonMass;
    [SerializeField] TextMeshProUGUI cannonVelocity;
    [SerializeField] TextMeshProUGUI ballMass;
    [SerializeField] TextMeshProUGUI ballVelocity;
    [SerializeField] TextMeshProUGUI explosionPower;

    Particle3D cannon;
    Particle3D ball;
    explosiveForce explosion;

    Vector3 cannonMaxVelocity;
    Vector3 ballMaxVelocity;

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
        explosionPower.text = "Power: " + explosion.explosionStrength.ToString();
        cannonMass.text = "Cannon Mass: " + cannon.getMass().ToString();
        ballMass.text = "Ball Mass: " + ball.getMass().ToString();

        if (cannonMaxVelocity.magnitude < cannon.getVelocity().magnitude)
        {
            cannonVelocity.text = "Cannon Start Velocity: -" + (cannon.getVelocity().magnitude * 50).ToString();
            cannonMaxVelocity = cannon.getVelocity();
        }
        if (ballMaxVelocity.magnitude < ball.getVelocity().magnitude)
        {
            ballVelocity.text = "Ball Start Velocity: " + (ball.getVelocity().magnitude * 50).ToString();
            ballMaxVelocity = ball.getVelocity();
        }
    }
}
