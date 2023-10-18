using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyPanel
{
    private VisualTreeAsset _lobbyAsset;

    private VisualElement _root;
    private Label _statusLabel;

    private ScrollView _lobbyScrollView;
    private bool _isLobbyRefresh = false;

    public LobbyPanel(VisualElement root, VisualTreeAsset lobbyAsset)
    {
        _root = root;
        _lobbyAsset = lobbyAsset;

        _statusLabel = root.Q<Label>("status-label");
        _lobbyScrollView = root.Q<ScrollView>("lobby-scroll");

        root.Q<Button>("btn-refresh")
            .RegisterCallback<ClickEvent>(HandleRefreshBtnClick);

    }

    private async void HandleRefreshBtnClick(ClickEvent evt)
    {
        if (_isLobbyRefresh) return;


        _isLobbyRefresh = true;
        var list = await ApplicationController.Instance.GetLobbyList();

        //
        foreach(var lobby in list)
        {
            var lobbyTemplate = _lobbyAsset.Instantiate();
            _lobbyScrollView.Add(lobbyTemplate);

            lobbyTemplate.Q<Label>("lobby-name").text = lobby.Name;
            lobbyTemplate.Q<Button>("btn-join")
                .RegisterCallback<ClickEvent>(evt =>
                {
                    try
                    {
                        // 여기서 조인하는 거 넣자.
                    } catch(Exception ex)
                    {

                    }
                });
        }
        //

        _isLobbyRefresh = false;
    }
}
