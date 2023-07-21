using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodScore : MonoBehaviour
{
    public Text scoreText;
    private Text scoreDisplayText;

    public int score;
    private int lastScore;

    private string scoreStr;

    // Start is called before the first frame update
    void Start()
    {
        score = 100000;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreText == null || lastScore == score)
            return;

        scoreStr = $"Score : {score}";
        scoreText.text = scoreStr;
        lastScore = score;
    }
}
