using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuScreen : MonoBehaviour
{
    private UIDocument _uiDocument;

    private VisualElement _contentElem;
    private readonly string _nameKey = "userName";

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;

        _contentElem = root.Q<VisualElement>("content");
        root.Q<VisualElement>("tab-container")
            .RegisterCallback<ClickEvent>(TabButtonClickHandle);

        root.Q<VisualElement>("popup-panel").RemoveFromClassList("off");

        var nameText = root.Q<TextField>("name-text");
        nameText.SetValueWithoutNotify(
            PlayerPrefs.GetString(_nameKey, string.Empty));

        //_contentElem

        root.Q<Button>("btn-ok")
            .RegisterCallback<ClickEvent>(e =>
            {
                string name = nameText.value;
                if (string.IsNullOrEmpty(name))
                    return;

                PlayerPrefs.SetString(_nameKey, name);
                root.Q<VisualElement>("popup-panel").AddToClassList("off");
            });
    }

    private void TabButtonClickHandle(ClickEvent evt)
    {
        if(evt.target is DataVisualElement)
        {
            var dve = evt.target as DataVisualElement;
            var percent = dve.panelIndex * -100;

            _contentElem.style.left = new Length(percent, LengthUnit.Percent);
        }
    }




}
