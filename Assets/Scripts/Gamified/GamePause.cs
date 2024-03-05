using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (isPaused)
        //        isPaused = false;
        //    else
        //        isPaused = true;
        //}
    }

    public bool getPauseState()
    {
        return isPaused;
    }
}
