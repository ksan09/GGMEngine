using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameText _textPrefab;
    [SerializeField] private GameHud _hud;

    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("Manager").GetComponent<UIManager>();
            if (_instance == null)
                Debug.LogError("No uimanager");

            return _instance;
        }
    }

    public void PopupText(string value, Vector3 pos, Color color)
    {
        var text = Instantiate(_textPrefab, pos, Quaternion.identity);
        text.SetUpText(value, pos, color);
    }

    public void ShowMsg(bool show, string msg = "")
    {
        _hud.ShowMsgContainer(show, msg);
    }

}
