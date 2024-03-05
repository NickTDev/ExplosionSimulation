using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnTarget : MonoBehaviour
{
    [SerializeField]
    GameObject target;
    Vector3 spawnPoint;


    private void OnTriggerEnter(Collider other)
    {
        Destroy(target);
        spawnPoint = new Vector3(Random.Range(-6.0f, 6.0f), Random.Range(-2.5f, 2.5f), 0.0f);
        Instantiate(target, spawnPoint, Quaternion.identity);
        GameObject.Find("Canvas").GetComponent<Score>().addScore(1);
    }
}
