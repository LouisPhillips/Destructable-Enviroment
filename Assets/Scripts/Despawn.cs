using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    private float despawnTimer = 0f;
    private float despawnMin = 7f;
    private float despawnMax = 12f;

    private bool hitGround = false;
    private void Update()
    {
        if (hitGround)
        {
            despawnTimer += Time.deltaTime;
            if (despawnTimer > Random.RandomRange(despawnMin, despawnMax))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            hitGround = true;
        }
    }
}
