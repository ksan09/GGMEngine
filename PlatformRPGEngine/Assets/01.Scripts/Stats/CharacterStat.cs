using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum StatType
{
    strength,       // ��
    agility,        // ��ø
    intelligence,   // ����
    vitality,       // �ǰ�
    maxHealth,
    armor,
    evasion,            // ȸ��
    magicResistance,
    damage,
    criticalChance,
    criticalDamage,
    fireDamage,
    ignitePercent,
    iceDamage,
    chillPercent,       // ����
    lightingDamage,
    shockPercent        // ����
}

public class CharacterStat : ScriptableObject
{
    [Header("Major stat")]
    public Stat strength; // 1����Ʈ�� ������ ����, ũ���� 1%
    public Stat agility; // 1����Ʈ�� ȸ�� 1%, ũ��Ƽ�� 1%
    public Stat intelligence; // 1����Ʈ�� ���������� 1����, �������� 3����, ��Ʈ �������� ������ 10% ����(����10�� ��Ʈ�� 10�� ����)
    public Stat vitality; // 1����Ʈ�� ü�� 5����.

    [Header("Defensive stats")]
    public Stat maxHealth; //ü��
    public Stat armor; //��
    public Stat evasion; //ȸ�ǵ�
    public Stat magicResistance; //�������

    [Header("Offensive stats")]
    public Stat damage;
    public Stat criticalChance;
    public Stat criticalDamage;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat ignitePercent;
    public Stat iceDamage;
    public Stat chillPercent;
    public Stat lightingDamage;
    public Stat shockPercent;

    protected Entity _owner;

    protected Dictionary<StatType, FieldInfo> _fieldInfoDictionary = new();

    public virtual void SetOwner(Entity owner)
    {
        _owner = owner;
    }

    public virtual void IncreaseStatBy(int modifyValue, float duration, Stat statToModify)
    {
        _owner.StartCoroutine(StatModifyCoroutine(modifyValue, duration, statToModify));
    }

    protected IEnumerator StatModifyCoroutine(int modifyValue, float duration, Stat statToModify)
    {
        statToModify.AddModifier(modifyValue);
        yield return new WaitForSeconds(duration);
        statToModify.RemoveModifier(modifyValue);
    }

    public int GetDamage()
    {
        return 0;
    }

    public bool CanEvasion()
    {
        return false;
    }

    public int ArmoredDamaged(int incomingDamage)
    {
        return 0;
    }

    public bool IsCritical(ref int incomingDamage)
    {
        return false;
    }

    protected int CalculateCriticalDamage(int incomingDamage)
    {
        return 0;
    }

    public virtual int GetMagicDamage()
    {
        return 0;
    }

    public int GetMaxHealthValue()
    {
        return 0;
    }

    public virtual int GetMagicDamageAfterRegist(int incomingDamage)
    {
        return 0;
    }
}
