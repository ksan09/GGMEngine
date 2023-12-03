using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stat/Player")]
public class PlayerStat : CharacterStat
{
    protected void OnEnable()
    {
        Type playerStatType = typeof(PlayerStat);

        foreach(StatType statType in Enum.GetValues(typeof(StatType)))
        {
            //string statName = statType.ToString() �������ϰ� ���� ����
            FieldInfo statField = playerStatType.GetField(statType.ToString());
            if(statField == null)
            {
                Debug.LogError($"There are no stat! error : {statField}");
            }
            else
            {
                _fieldInfoDictionary.Add(statType, statField);
            }
        }
    }

    public Stat GetStatByType(StatType statType)
    {
        return _fieldInfoDictionary[statType].GetValue(this) as Stat;
    }

    

}
