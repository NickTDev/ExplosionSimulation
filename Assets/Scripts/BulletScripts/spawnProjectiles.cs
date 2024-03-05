using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnProjectiles : MonoBehaviour
{
    [SerializeField]
    GameObject toSpawn;
    [SerializeField]
    int numToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        spawnRandomProjectiles();
    }

    void spawnRandomProjectiles()
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            Vector3 spawnPosition;
            //Between -11 and 11 on x
            spawnPosition.x = Random.Range(-11.0f, 11.0f);
            //Between -3 and 5 on y
            spawnPosition.y = Random.Range(-3.0f, 5.0f);
            //Between -19 and 19 on z
            spawnPosition.z = Random.Range(-19.0f, 19.0f);

            Instantiate(toSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
