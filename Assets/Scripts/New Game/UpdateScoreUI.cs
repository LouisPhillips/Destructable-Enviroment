using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpdateScoreUI : MonoBehaviour
{
    void Update()
    {
        GetComponent<Text>().text = "Score: " + GameController.score.ToString();
    }
}
