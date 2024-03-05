using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("destroySelf", 0.1f);
    }

    void destroySelf()
    {
        Destroy(gameObject);
    }
}
