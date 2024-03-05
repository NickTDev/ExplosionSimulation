using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    static public int currentScore = 0;
    [SerializeField]
    TMP_Text scoreText;
    SpatialPartitioning spManager;
    BinarySpacePartitioning bspManager;

    private void Awake()
    {
        spManager = GameObject.Find("GameStateManager").GetComponent<SpatialPartitioning>();
        bspManager = GameObject.Find("ParticleManager").GetComponent<BinarySpacePartitioning>();
    }

    public void addScore(int toAdd)
    {
        currentScore += toAdd;
        scoreText.text = currentScore.ToString();
    }

    public void setScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void calcScore()
    {
        currentScore = 0;
        if (spManager.getSP())
        {
            currentScore = bspManager.getNumCollisions();
            bspManager.resetNumCollisions();
        }
        else {
            Particle3D[] particles = FindObjectsOfType<Particle3D>();
            for (int i = 0; i < particles.Length; i++)
            {
                currentScore += particles[i].getNumCollisions();
                particles[i].resetNumCollisions();
            }
        }

        setScore(currentScore);
    }

    private void FixedUpdate()
    {
        calcScore();
    }
}
