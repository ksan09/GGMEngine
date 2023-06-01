using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour,IItem
{
    public int score = 10; // 증가할 점수

    public void Use(GameObject target)
    {

        GameManager.instance.AddScore(score);
        Destroy(gameObject);
    }

}
