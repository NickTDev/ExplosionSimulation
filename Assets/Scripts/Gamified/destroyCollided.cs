using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyCollided : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Wall")
        {
            Destroy(other.gameObject);
        }
    }
}
