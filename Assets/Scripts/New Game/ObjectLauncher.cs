using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLauncher : MonoBehaviour
{
    private Rigidbody rb;
    private bool idle = true;

    float movement;
    public float speed = 1;
    public GameObject startPos;
    public bool getNewZ;
    public static float newZ;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);

        movement += Time.deltaTime * GameController.speedChange;

        movement = movement % 5f;

        transform.position = MathParabola.Parabola(new Vector3(startPos.transform.position.x, startPos.transform.position.y, newZ), -Vector3.right * 10f, 7f, movement / 5f);
    }
}
