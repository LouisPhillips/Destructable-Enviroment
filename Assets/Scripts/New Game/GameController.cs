using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int score = 0;
    public static float speedChange = 1f;
    public static int lives = 3;
    public static int scoreMultiplier = 1;

    private float timeMax = 5f;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeMax)
        {
            scoreMultiplier = scoreMultiplier * 2;
            timer = 0f;
        }

        if (lives < 0)
        {
            speedChange = 1f;
            scoreMultiplier = 1;
            timer = 0f;
            score = 0;
            lives = 3;
        }
    }
}
