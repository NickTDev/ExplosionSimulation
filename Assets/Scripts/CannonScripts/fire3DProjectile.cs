using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire3DProjectile : MonoBehaviour
{
    [SerializeField]
    GameObject shotType1;
    [SerializeField]
    GameObject shotType2;
    [SerializeField]
    GameObject shotType3;
    [SerializeField]
    GameObject shotType4;
    [SerializeField]
    Transform spawnPoint;
    int currentShotType;
    int numShotTypes;
    float reloadTime = 3.5f;
    bool hasShot = false;

    // Start is called before the first frame update
    void Start()
    {
        currentShotType = 0;
        numShotTypes = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasShot)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (currentShotType == 0)
                {
                    Instantiate(shotType1, spawnPoint.position, Quaternion.identity);
                    hasShot = true;
                }
                else if (currentShotType == 1)
                {
                    Instantiate(shotType2, spawnPoint.position, Quaternion.identity);
                    hasShot = true;
                }
                else if (currentShotType == 2)
                {
                    Instantiate(shotType3, spawnPoint.position, Quaternion.identity);
                    hasShot = true;
                }
                else if (currentShotType == 3)
                {
                    Instantiate(shotType4, spawnPoint.position, Quaternion.identity);
                    hasShot = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            currentShotType = (currentShotType + 1) % numShotTypes;
        }

        if (hasShot)
        {
            if (reloadTime > 0)
            {
                reloadTime -= Time.deltaTime;
            }
            else if (reloadTime <= 0)
            {
                reloadTime = 3.5f;
                hasShot = false;
            }
        }
    }
}
