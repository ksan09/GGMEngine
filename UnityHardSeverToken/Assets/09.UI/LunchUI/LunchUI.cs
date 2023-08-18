using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LunchUI : WindowUI
{
    private TextField _dateTextField;
    private Label _lunchLabel;

    public LunchUI(VisualElement root) : base(root)
    {
        _dateTextField = root.Q<TextField>(name: "DateTextField");
        _lunchLabel = root.Q<Label>(name: "LunchLabel");
        root.Q<Button>(name: "LoadBtn").RegisterCallback<ClickEvent>(OnLoadButtonHandle);
        root.Q<Button>(name: "CloseBtn").RegisterCallback<ClickEvent>(OnCloseButtonHandle);
    }

    private void OnLoadButtonHandle(ClickEvent evt)
    {
        string dateStr = _dateTextField.value;
        Regex regex = new Regex(@"20[0-9]{2}[0-1][0-9][0-3][0-9]");
        if(!regex.IsMatch(dateStr))
        {
            UIController.Instance.Message.AddMessage("�ùٸ��� �ʾƿ�. ���� 8�ڸ��� �Է��ϼ���.", 3f);
            return;
        }

        NetworkManager.Instance.GetRequest("lunch", $"?date={dateStr}", (type, json) =>
        {
            string menuStr = "";

            if (type == MessageType.SUCCESS)
            {
                LunchVO vo = JsonUtility.FromJson<LunchVO>(json);
                menuStr = vo.menus.Aggregate("", (sum, x) => sum + x + '\n');
            }
            else
            {
                menuStr = "�ҷ����� ����";
            }

            if (menuStr == "")
                menuStr = "�޴� ����";

            _lunchLabel.text = menuStr;
        });
        
    }
    private void OnCloseButtonHandle(ClickEvent evt)
    {
        //Do nothing
        Close();
    }
}
