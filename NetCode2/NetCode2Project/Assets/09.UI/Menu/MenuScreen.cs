using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuScreen : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset _createPanelAsset;
    [SerializeField] private VisualTreeAsset _lobbyPanelAsset;
    [SerializeField] private VisualTreeAsset _lobbyTemplateAsset;

    private UIDocument _uiDocument;

    private VisualElement _contentElem;
    private const string _nameKey = "userName";

    private bool _isCreatingLobby = false; //로비가 생성중인가?
    private CreatePanel _createPanel;
    private LobbyPanel _lobbyPanel;

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

        root.Q<Button>("btn-ok")
            .RegisterCallback<ClickEvent>(e =>
            {
                string name = nameText.value;
                if (string.IsNullOrEmpty(name))
                    return;

                PlayerPrefs.SetString(_nameKey, name);
                root.Q<VisualElement>("popup-panel").AddToClassList("off");
            });

        // 크리에이트 패널 만들기
        var createPanel = _createPanelAsset.Instantiate();
        createPanel.AddToClassList("panel");
        root.Q<VisualElement>("page-one").Add(createPanel);
        _createPanel = new CreatePanel(createPanel);
        _createPanel.MakeLobbyBtnEvent += HandleCreateLobby;
        //

        //로비 패널 만들기
        var lobbyPanel = _lobbyPanelAsset.Instantiate();
        lobbyPanel.AddToClassList("panel");
        root.Q<VisualElement>("page-two").Add(lobbyPanel);
        _lobbyPanel = new LobbyPanel(lobbyPanel, _lobbyTemplateAsset);
        //

    }

    private async void HandleCreateLobby(string lobbyName)
    {
        if (_isCreatingLobby)
            return;

        if(string.IsNullOrEmpty(lobbyName))
        {
            _createPanel.SetStatusText("로비 이름은 공백일 수 없습니다.");
            return;
        }

        string username = PlayerPrefs.GetString(_nameKey);
        //여기서는 스테이터스 텍스트에 로딩 텍스트가 나오게 만들어놓고 가면 된다.

        _isCreatingLobby = true;
        LoadText(_createPanel.StatusLabel);
        bool result = await ApplicationController.Instance.StartHost(username, lobbyName);
        if (result)
        {
            NetworkManager.Singleton.SceneManager
                .LoadScene(SceneList.GameScene, LoadSceneMode.Single);
        }
        else
        {
            _createPanel.SetStatusText("로비 생성중 오류 발생");
        }
       
        _isCreatingLobby = false;
    }

    private async void LoadText(Label targetLabel)
    {
        string[] makings = { "Loading", "Loading.", 
            "Loading..", "Loading...", "Loading...." };

        int idx = 0;
        while(_isCreatingLobby)
        {
            targetLabel.text = makings[idx];
            idx = (idx + 1) % makings.Length;
            await Task.Delay(300);
        }
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
