using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCollUI : MonoBehaviour
{
    [SerializeField] private PlayerSkill _skillType;
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private TextMeshProUGUI _coolTimeText;

    private Skill _skill;

    private void Start()
    {
        _skill = SkillManager.Instance.GetSkill(_skillType);
        _cooldownImage.fillAmount = 0;
        _skill.OnCoolDown += HandleCoolDown;
    }

    private void HandleCoolDown(float current, float total)
    {
        _cooldownImage.fillAmount = current / total;

        if (current < 0.01f)
            _coolTimeText.text = "";
        else
            _coolTimeText.text = current.ToString("0.##");
        
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(_skillType != 0)
        {
            gameObject.name = $"SkillCoolUI[{_skillType}]";
        }
    }
#endif
}
