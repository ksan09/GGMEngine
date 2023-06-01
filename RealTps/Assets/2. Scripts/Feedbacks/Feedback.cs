using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Feedback : MonoBehaviour
{
    public abstract void CreateFeedback();
    public abstract void FinishFeedback();

    private void OnDisable()
    {
        FinishFeedback();
    }
}
