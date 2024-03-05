using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialPartitioning : MonoBehaviour
{
    bool spIsOn;

    private void Start()
    {
        spIsOn = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (spIsOn)
                spIsOn = false;
            else
                spIsOn = true;
        }
    }

    public bool getSP()
    {
        return spIsOn;
    }
}
