using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSkill
{
    //None = 0,
    Dash = 1,
    Clone = 2,
    //Crystal = 3,
}

public class SkillManager : MonoSingleton<SkillManager>
{
    private Dictionary<Type, Skill> _skills;
    private Dictionary<PlayerSkill, Type> _skillTypeDictionary;

    private void Awake()
    {
        _skills = new Dictionary<Type, Skill>();
        _skillTypeDictionary = new Dictionary<PlayerSkill, Type>();

        foreach(PlayerSkill skill in Enum.GetValues(typeof(PlayerSkill)))
        {
            Skill skillCompo = GetComponent($"{skill.ToString()}Skill") as Skill;
            Type skillType = skillCompo.GetType();
            _skills.Add(skillType, skillCompo);
            _skillTypeDictionary.Add(skill, skillType);
        }
    }

    public T GetSkill<T>() where T : Skill
    {
        Type t = typeof(T);
        if(_skills.TryGetValue(t, out Skill targetSkill))
        {
            return targetSkill as T;
        }
        return null;
    }

    public Skill GetSkill(PlayerSkill skillEnumType)
    {
        Type skillType = _skillTypeDictionary[skillEnumType];
        if(_skills.TryGetValue(skillType, out Skill skill))
        {
            return skill;
        }
        return null;
    }

    public void UseSkillFeedback(PlayerSkill skill)
    {
        ItemDataEquipmentSO amulet = 
            Inventory.Instance.equipmentWindow.GetEquipmentByType(EquipmentType.Amulet);

        if(amulet != null)
        {
            // «ÿ¥Á æ∆πƒ∑ø¿« ¿Ã∆Â∆Æ∏¶ πﬂµøΩ√≈≤¥Ÿ.
        }
    }
}
