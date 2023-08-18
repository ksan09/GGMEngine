using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Windows
{
    Lunch = 0,
}

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private VisualTreeAsset _lunchUIAsset; //UIÀÇ ÇÁ¸®ÆÕ

    private UIDocument _uiDocument;
    private VisualElement _contentParent;

    private MessageSystem _messagSystem;
    public MessageSystem Message => _messagSystem;

    private Dictionary<Windows, WindowUI> _windowDictinary = new Dictionary<Windows, WindowUI>();


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple UI Manager is running");
        }
        Instance = this;

        _uiDocument = GetComponent<UIDocument>();
        _messagSystem = GetComponent<MessageSystem>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;

        _contentParent = _uiDocument.rootVisualElement.Q<VisualElement>("Content");

        Button lunchBtn = _uiDocument.rootVisualElement.Q<Button>("LunchBtn");
        lunchBtn.RegisterCallback<ClickEvent>(OnOpenLunchHandle);

        var messageContainer = root.Q<VisualElement>("MessageContainer");
        _messagSystem.SetContainer(messageContainer);

        //
        _windowDictinary.Clear();
        VisualElement lunchRoot = _lunchUIAsset.Instantiate().Q<VisualElement>("LunchContainer");
        _contentParent.Add(lunchRoot);
        LunchUI lunchUI = new LunchUI(lunchRoot);
        lunchUI.Close();

        _windowDictinary.Add(Windows.Lunch, lunchUI);
        //

        Button inventoryBtn = _uiDocument.rootVisualElement.Q<Button>("InventoryBtn");
        Button loginBtn = _uiDocument.rootVisualElement.Q<Button>("LoginBtn");
    }

    private void OnOpenLunchHandle(ClickEvent evt)
    {
        foreach(var kvPair in _windowDictinary)
        {
            kvPair.Value.Close();
        }
        _windowDictinary[Windows.Lunch].Open();
    }
}
