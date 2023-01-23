using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMovement : MonoBehaviour
{
    Vector3 mousePos;
    public float offset = 3f;
    float oppositeAngle = 0;
    public float turnRotation = 15f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Rotate(Vector3.left * turnRotation, Space.Self);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.Rotate(Vector3.right * turnRotation, Space.Self);
        }

        mousePos = Input.mousePosition;
        mousePos.z = offset;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);

        oppositeAngle = (transform.rotation.x - 180f);

        //Debug.Log(transform.rotation.x * 180);
    }
}
