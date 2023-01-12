using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    private float despawnTimer = 0f;
    private float despawnMin = 7f;
    private float despawnMax = 12f;

    private float fadeTimer = 0f;
    private float fadeMax = 3f;

    private bool hitGround = false;
    private void Update()
    {
        if (hitGround)
        {
            despawnTimer += Time.deltaTime;
            if (despawnTimer > Random.RandomRange(despawnMin, despawnMax))
            {
                var colour = gameObject.GetComponent<Renderer>().material.color.a;

                colour += colour -1000 * Time.deltaTime;

                Debug.Log(colour);
                //Destroy(gameObject);
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
