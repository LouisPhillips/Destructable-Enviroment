using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    private float despawnTimer = 0f;
    private float despawnMin = 3f;
    private float despawnMax = 5f;

    private void Update()
    {

        despawnTimer += Time.deltaTime;
        if (despawnTimer > Random.RandomRange(despawnMin, despawnMax))
        {
            Destroy(gameObject);
        }

    }


}
