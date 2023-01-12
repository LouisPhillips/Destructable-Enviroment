using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMovement : MonoBehaviour
{
    Vector3 mousePos;
    public float offset = 3f;
    float oppositeAngle = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Rotate(Vector3.left * 15f, Space.Self);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.Rotate(Vector3.right * 15f, Space.Self);
        }

        mousePos = Input.mousePosition;
        mousePos.z = offset;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);

        oppositeAngle = (transform.rotation.x - 180f);

        Debug.Log(transform.rotation.x * 180);
    }
}
