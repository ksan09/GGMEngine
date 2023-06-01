using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadFeedback : Feedback
{
    private Animator anim;

    private void Awake()
    {
        anim = transform.parent.GetComponent<Animator>();
    }

    public override void CreateFeedback()
    {
        anim.SetTrigger("Death");
    }

    public override void FinishFeedback()
    {
        Debug.Log("the end");
    }

}
