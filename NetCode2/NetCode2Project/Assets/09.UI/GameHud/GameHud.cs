﻿using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHud : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _startGameBtn;
    private Button _readyGameBtn;
    private List<PlayerUI> _players = new();

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;
        _startGameBtn = root.Q<Button>("btn-start");
        _readyGameBtn = root.Q<Button>("btn-ready");

        root.Query<VisualElement>(className: "player").ToList().ForEach(x =>
        {
            var player = new PlayerUI(x);
            _players.Add(player);
            player.RemovePlayerUI();
        });

        _startGameBtn.RegisterCallback<ClickEvent>(HandleGameStartClick);
        _readyGameBtn.RegisterCallback<ClickEvent>(HandleReadyClick);
    }

    // 여기 왔을 때는 게임매니저가 다 완성됨, 네트워크 스폰은 안 됨
    private void Start()
    {
        GameManager.Instance.players.OnListChanged  += HandlePlayerListChanged;
        GameManager.Instance.GameStateChanged       += HandleGameStateChanged;

        //이때 이미 네트워크 리스트에 한명은 들어가 있음. 호스트
        foreach(GameData data in GameManager.Instance.players)
        {
            HandlePlayerListChanged(new NetworkListEvent<GameData>
            {
                Type = NetworkListEvent<GameData>.EventType.Add,
                Value = data
            });
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.players.OnListChanged  -= HandlePlayerListChanged;
        GameManager.Instance.GameStateChanged       -= HandleGameStateChanged;
    }

    private void HandlePlayerListChanged(NetworkListEvent<GameData> evt)
    {
        Debug.Log($"{evt.Type}, {evt.Value.clientID}");
    }

    private void HandleGameStateChanged(GameState obj)
    {
        
    }

    private void HandleGameStartClick(ClickEvent evt)
    {
        //

    }

    private void HandleReadyClick(ClickEvent evt)
    {
        //

    }
}
