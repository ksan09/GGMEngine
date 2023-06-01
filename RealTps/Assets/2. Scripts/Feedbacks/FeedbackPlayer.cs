using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackPlayer : MonoBehaviour
{
    private List<Feedback> _playFeedbacList;

    private void Awake()
    {
        _playFeedbacList = new List<Feedback>();
        GetComponentsInChildren<Feedback>(_playFeedbacList);
    }

    public void PlayFeedback()
    {
        FinishFeedback();
        foreach(Feedback f in _playFeedbacList)
        {
            f.CreateFeedback();
        }
        
    }

    private void FinishFeedback()
    {
        foreach (Feedback f in _playFeedbacList)
        {
            f.FinishFeedback();
        }
    }
}
