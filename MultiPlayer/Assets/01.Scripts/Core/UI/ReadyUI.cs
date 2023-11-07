using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class ReadyUI : MonoBehaviour
{
    public event Action<int> TankSelect;
    public event Action<bool> ReadyChanged;

    public event Action GameStarted;

    private UIDocument _uiDocument;
    private VisualElement _selectPanel;
    private Label _selectedTankNameLabel;
    private Label _selectedTankDescLabel;

    private Button _readyBtn;
    private Button _startBtn;

    // 현재 내 클라이언트가 레디인지를 넣는 것이다.
    private bool _isReady = false;

    private VisualElement _readyList; //
    private Dictionary<int, TankDataSO> _tankDataDictionary = new();
    private Dictionary<ulong, UserUI> _userDictionary = new();

    [SerializeField]
    private VisualTreeAsset _tankTemplate;
    [SerializeField]
    private VisualTreeAsset _userTemplate;

    private List<TankUI> _tankUIList = new List<TankUI>();

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = _uiDocument.rootVisualElement;

        _selectPanel = root.Q<VisualElement>("select-panel");
        _selectPanel.RegisterCallback<ClickEvent>(HandleTankClick);

        _selectPanel.Clear(); //기존에 만들어져 있는 더미 빨강 탱크를 다 지워준다.

        _selectedTankNameLabel = root.Q<Label>("tank-name");
        _selectedTankDescLabel = root.Q<Label>("tank-desc"); //description

        // 입장 유저들 나타날 곳
        _readyList = root.Q<VisualElement>("ready-list");
        _readyList.Clear();

        // 버튼 제어 관련 로직
        _readyBtn = root.Q<Button>("btn-ready");
        _startBtn = root.Q<Button>("btn-start");

        // 여기에 각각 버튼을 클릭했을 때의 동작이 들어가야 한다.
        _readyBtn.RegisterCallback<ClickEvent>(HandleReady);
    }

    private void HandleTankClick(ClickEvent evt)
    {
        if (_isReady) return;

        TankVisualElement tankElement = evt.target as TankVisualElement;
        if(tankElement != null)
        {
            TankDataSO tankData = _tankDataDictionary[tankElement.tankIndex];
            _selectedTankNameLabel.text = tankData.tankName;

            string desc = $"상세 데이터\n " +
                $"기본 공격력 : {tankData.basicTurretSO.damage}\n " +
                $"이동속도 : {tankData.moveSpeed}\n " +
                $"최대체력 : {tankData.maxHP}\n " +
                $"회전속도 : {tankData.rotateSpeed}";

            _selectedTankDescLabel.text = desc;

            TankSelect?.Invoke(tankData.tankID);
        }
    }
    private void HandleReady(ClickEvent evt)
    {
        _isReady = !_isReady;
        ReadyChanged?.Invoke(_isReady);
    }

    public void SetTankTemplate(List<TankDataSO> list)
    {
        _selectPanel.Clear();
        _tankUIList.Clear();

        foreach(TankDataSO tank in list)
        {
            _tankDataDictionary.Add(tank.tankID, tank);
            TemplateContainer template = _tankTemplate.Instantiate();
            _selectPanel.Add(template);

            TankUI tankUI = new TankUI(template, tank);
            _tankUIList.Add(tankUI);
        }
    }

    private void OnDisable()
    {
        
    }

    public void AddUserData(UserListEntityState userData)
    {
        // 중복 처리 무시
        if(_userDictionary.ContainsKey(userData.clientID))
        {
            return;
        }

        Sprite sprite = userData.tankID != 0 ? _tankDataDictionary[userData.tankID].bodySprite : null;
        string name = userData.playerName.Value;
        TemplateContainer template = _userTemplate.Instantiate();
        UserUI userUI = new UserUI(template, name, sprite, userData.ready);

        _readyList.Add(template);
        _userDictionary.Add(userData.clientID, userUI);
    }

    public void RemoveUserData(UserListEntityState userData)
    {
        UserUI userUI = _userDictionary[userData.clientID];
        userUI.RemoveFromTree();
    }

    public void UpdateUserData(UserListEntityState userData)
    {
        Sprite sprite = userData.tankID != 0 ? _tankDataDictionary[userData.tankID].bodySprite : null;
        UserUI userUI = _userDictionary[userData.clientID];
        userUI.SetTank(sprite);
        userUI.SetReady(userData.ready);


    }
}
