using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForMiss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameController.lives -= 1;
    }
}
