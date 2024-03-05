using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropPayload : MonoBehaviour
{
    [SerializeField] 
    GameObject toSpawn;

    float timeLeft = 1;
    bool hasDropped;

    // Start is called before the first frame update
    void Start()
    {
        hasDropped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else if (timeLeft <= 0 && !hasDropped)
        {
            Instantiate(toSpawn, transform.position, Quaternion.identity);
            hasDropped = true;
        }
    }
}
