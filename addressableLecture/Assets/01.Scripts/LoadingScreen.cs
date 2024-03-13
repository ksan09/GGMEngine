using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    UIDocument _document;
    VisualElement _root;
    private Label _titleLabel;
    private Label _descLabel;
    private VisualElement _loadingComplete;

    private bool _complete = false;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        
    }

    private void OnEnable()
    {
        _root = _document.rootVisualElement;
        _titleLabel = _root.Q<Label>("info-text");
        _descLabel = _root.Q<Label>("info-text2");
        _loadingComplete = _root.Q<VisualElement>("load-complete");

        _loadingComplete.style.visibility = Visibility.Hidden;
        _complete = false;

        AssetLoader.OnCategoryMessage   -= HandleCategoryMsg;
        AssetLoader.OnDescMessage       -= HandleDescMsg;
        AssetLoader.OnLoadComplete      -= HandleLoadMsg;
    }

    private void OnDisable()
    {
        AssetLoader.OnCategoryMessage   -= HandleCategoryMsg;
        AssetLoader.OnDescMessage       -= HandleDescMsg;
        AssetLoader.OnLoadComplete      -= HandleLoadMsg;
    }

    private void Update()
    {
        if(_complete)
        {
            if(Input.anyKeyDown)
            {
                SceneManager.LoadScene(SceneList.Menu);
            }
        }
    }

    private void HandleCategoryMsg(string message)
    {
        _titleLabel.text = message;
    }

    private void HandleDescMsg(string message)
    {
        _descLabel.text = message;
    }

    private void HandleLoadMsg()
    {
        _titleLabel.text = "Load Complete";
        _descLabel.text = "게임을 시작합니다.";

        _loadingComplete.style.visibility = Visibility.Visible;
        _complete = true;
    }
}
