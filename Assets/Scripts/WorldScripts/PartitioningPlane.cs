using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartitioningPlane : MonoBehaviour
{
    public Vector3 position;
    public Vector3 direction;

    private void Start()
    {
        position = transform.position;
        direction = transform.up;
    }
}
