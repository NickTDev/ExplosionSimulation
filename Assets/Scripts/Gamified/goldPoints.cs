using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldPoints : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Wall")
        {
            Destroy(this.gameObject);
            GameObject.Find("Canvas").GetComponent<Score>().addScore(10);
        }
    }
}
