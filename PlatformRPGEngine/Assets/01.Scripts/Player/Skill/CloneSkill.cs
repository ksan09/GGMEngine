using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill, ISaveManager
{
    [Header("Clone Info")]
    [SerializeField] private CloneSkillController _clonePrefab;
    [SerializeField] private bool _createCloneOnDashStart;
    [SerializeField] private bool _createCloneOnDashEnd;
    [SerializeField] private bool _createCloneOnCounterAttack;

    public float cloneDuration;

    public float findEnemyRadius = 5f;

    public void CreateClone(Transform originTrm, Vector3 offset)
    {
        CloneSkillController newClone = Instantiate(_clonePrefab);
        newClone.SetUpClone(this, originTrm, offset);
    }

    public void CreateCloneOnDashStart()
    {
        if(_createCloneOnDashStart)
        {
            CreateClone(_player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashEnd()
    {
        if (_createCloneOnDashEnd)
        {
            CreateClone(_player.transform, Vector3.zero);
        }
    }

    public void LoadData(GameData data)
    {
        //
        if (data.skillTree.TryGetValue("clone_enable", out int value))
            skillEnalbled = (value == 1);

        if (data.skillTree.TryGetValue("clone_start", out int startValue))
            _createCloneOnDashStart = (startValue == 1);

        if (data.skillTree.TryGetValue("clone_end", out int endValue))
            _createCloneOnDashEnd = (endValue == 1);

    }

    public void SaveData(ref GameData data)
    {
        if (data.skillTree.ContainsKey("clone_enable"))
            data.skillTree.Remove("clone_enable");

        if (data.skillTree.ContainsKey("clone_start"))
            data.skillTree.Remove("clone_start");

        if (data.skillTree.ContainsKey("clone_end"))
            data.skillTree.Remove("clone_end");

        data.skillTree.Add("clone_enable", skillEnalbled ? 1 : 0);
        data.skillTree.Add("clone_start", _createCloneOnDashStart ? 1 : 0);
        data.skillTree.Add("clone_end", _createCloneOnDashEnd ? 1 : 0);
    }
}
