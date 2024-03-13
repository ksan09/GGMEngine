using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>, ISaveManager
{
    [SerializeField] private int _currentGold;
    [SerializeField] private int _currentExp;

    public void LoadData(GameData data)
    {
        _currentGold = data.gold;
        _currentExp = data.exp;
    }

    public void SaveData(ref GameData data)
    {
        data.gold = _currentGold;
        data.exp = _currentExp;
    }
}
