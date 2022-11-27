using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTest : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.AddForce(transform.right * 2500000 * Time.deltaTime);
        float getSpeed = rb.velocity.magnitude;
        float maxSpeed = 500f;
        Vector3 vel = rb.velocity;
        if (vel.magnitude > maxSpeed)
        {
            rb.velocity = vel.normalized * maxSpeed;
        }
        if (Input.GetMouseButtonDown(0))
        {
            transform.position = new Vector3(10.08f, Random.RandomRange(1f, 3f), Random.RandomRange(-1.27f, 9f));
        }
    }
}
