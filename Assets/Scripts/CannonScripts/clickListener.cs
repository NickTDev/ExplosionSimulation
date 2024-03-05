using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickListener : MonoBehaviour
{
    [SerializeField]
    GameObject toSpawn;
    Vector3 mousePos;
    public Vector3 worldPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
            worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 10;
            Instantiate(toSpawn, worldPos, Quaternion.identity);
        }
    }
}
