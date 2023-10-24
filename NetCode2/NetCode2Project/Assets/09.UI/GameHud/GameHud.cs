using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHud : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _startGameBtn;
    private Button _readyGameBtn;
    private List<PlayerUI> _players = new();

    private VisualElement _container;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;
        _container = root.Q<VisualElement>("container");
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

    private bool CheckPlayerExist(ulong clientID)
    {
        return _players.Any(x => x.clientID == clientID);
    }

    private PlayerUI FindEmptyPlayerUI()
    {
        foreach(var playerUI in _players)
        {
            if (playerUI.clientID == 999)
            {
                return playerUI;
            }
        }
        return null;
    }

    private void HandlePlayerListChanged(NetworkListEvent<GameData> evt)
    {
        switch (evt.Type)
        {
            case NetworkListEvent<GameData>.EventType.Add:
                {
                    if(!CheckPlayerExist(evt.Value.clientID))
                    {
                        var playerUI = FindEmptyPlayerUI();
                        playerUI.SetGameData(evt.Value);
                        playerUI.SetColor(GameManager.
                            Instance.slimeColors[evt.Value.colorIdx]);
                        playerUI.VisiblePlayerUI();
                    }
                }
                break;
            case NetworkListEvent<GameData>.EventType.Remove:
                {
                    var playerUI = _players.Find(x => x.clientID == evt.Value.clientID);
                    playerUI.RemovePlayerUI();
                }
                break;
            case NetworkListEvent<GameData>.EventType.Value:
                {
                    //플레이어 UI 찾, SetCheck 알잘딱 실행
                    var playerUI = _players.Find(x => x.clientID == evt.Value.clientID);
                    playerUI.SetCheck(evt.Value.ready);
                }
                break;
            default:
                break;
        }
    }

    private void HandleGameStateChanged(GameState obj)
    {
        if(obj == GameState.Game)
        {
            _container.AddToClassList("off");
        }
    }

    private void HandleGameStartClick(ClickEvent evt)
    {
        //
        if(GameManager.Instance.myGameRole != GameRole.Host)
        {
            Debug.Log("게임 호스트만 게임 시작이 가능합니다.");

            return;
        }
        //게임 시작 로직 제작
        GameManager.Instance.GameStart();
    }

    private void HandleReadyClick(ClickEvent evt)
    {
        //
        GameManager.Instance.GameReady();
    }
}
