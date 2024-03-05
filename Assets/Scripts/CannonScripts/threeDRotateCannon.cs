using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class threeDRotateCannon : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    Vector3 direction;
    Vector3 mousePos;

    private void Update()
    {
        //get vector of ScreentoWorldPoint(Input.mousePosition) and cannonPos
        //set z to a value
        //use this to set cannon.transform.up

        mousePos = Input.mousePosition;
        mousePos.z = -5;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);

        direction = mousePos - this.transform.position;

        this.transform.up = -direction;
    }
}
