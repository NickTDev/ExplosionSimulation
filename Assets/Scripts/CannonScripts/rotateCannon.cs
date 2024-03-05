using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCannon : MonoBehaviour
{
    [SerializeField]
    GameObject cannon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            cannon.transform.Rotate(0.0f, 0.0f, 1.0f, Space.Self);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            cannon.transform.Rotate(0.0f, 0.0f, -1.0f, Space.Self);
        }
    }
}
