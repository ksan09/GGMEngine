using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private CrystalSkillController _crystalPrefab;
    public float timeOut = 5f;

    private CrystalSkillController _currentCrystal;

    public int damage = 5;

    public bool canExplode;
    public float explosionRadius = 3f;

    public bool canMove;
    public float moveSpeed;
    public float findEnemyRadius = 10f;

    public override bool AtemptUseSkill()
    {
        if(_cooldownTimer <= 0 && skillEnalbled && _currentCrystal == null)
        {
            UseSkill();
            return true;
        }

        if (_currentCrystal != null)
        {
            WarpToCrystalPosition();
        }

        Debug.Log("Skill CoolDown or Locked");
        return false;
    }

    private void WarpToCrystalPosition()
    {
        Transform pTrm = _player.transform;
        (pTrm.position, _currentCrystal.transform.position) =
            ( _currentCrystal.transform.position, pTrm.position );
        _currentCrystal.EndOfCrystal();
    }

    public void UnlinkCrystal()
    {
        _cooldownTimer = _cooldown;
        _currentCrystal = null;
    }

    public override void UseSkill()
    {
        base.UseSkill();

         if(_currentCrystal == null)
        {
            CreateCrystal(_player.transform.position);
        }
    }

    private void CreateCrystal(Vector3 position)
    {
        _currentCrystal = Instantiate(_crystalPrefab, position, Quaternion.identity);
        _currentCrystal.SetUpCrystal(this);
    }


}
