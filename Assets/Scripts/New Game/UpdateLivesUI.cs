using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLivesUI : MonoBehaviour
{
    void Update()
    {
        GetComponent<Text>().text = "Lives: " + GameController.lives.ToString();
    }
}
