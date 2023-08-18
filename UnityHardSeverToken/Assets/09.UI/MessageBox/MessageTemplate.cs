using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageTemplate
{
    private VisualElement _root;
    public VisualElement Root => _root;

    private Label _label;
    private float _timer = 0;
    private float _currentTimer = 0;
    private bool _fade = false;
    private bool _isComplete = false;
    public bool IsComplete => _isComplete;

    public string Text
    {
        get => _label.text;
        set => _label.text = value;
    }

    public MessageTemplate(VisualElement root, float timer)
    {
        _root = root;
        _label = root.Q<Label>("Msg");
        _currentTimer = 0;
        _fade = false;
        _timer = timer;
    }

    public void UpdateMessage()
    {
        _currentTimer += Time.deltaTime;
        if (_currentTimer >= _timer && !_fade)
        {
            _root.AddToClassList("off");
            _fade = true;
        }

        if (_currentTimer >= _timer + 0.6f)   //트랜지션 시간까지 해서 사라지는게 완료되면
        {
            _isComplete = true;
        }
    }
}
