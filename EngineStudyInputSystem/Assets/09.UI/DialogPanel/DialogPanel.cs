using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogPanel
{
    private VisualElement _element;
    private Transform _body;

    private Label _textLabel;

    public string Text
    {
        get => _textLabel.text;
        set => _textLabel.text = value;
    }

    public DialogPanel(VisualElement element, string text, Transform body)
    {
        _element = element;
        _textLabel = _element.Q<Label>("message-label");
        _body = body;

        Text = text;
    }

    public void LookRotation(Quaternion dir)
    {
        _body.rotation = dir;
    }

    public void SetOn(bool val)
    {
        if(val)
        {
            _element.AddToClassList("on");
        }
        else
        {
            _element.RemoveFromClassList("on");

        }
    }
}
