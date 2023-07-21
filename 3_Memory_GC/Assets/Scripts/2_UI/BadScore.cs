using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadScore : MonoBehaviour
{
    public Text scoreText;

    public int score;

    private string scoreStr;

    // Start is called before the first frame update
    void Start()
    {
        score = 100000;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreText == null)
            return;

        scoreStr = "Score : " + score.ToString();
        scoreText.text = scoreStr;
    }
}
