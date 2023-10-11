using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletUI
{
    private VisualElement _element;
    private int _bulletCount;

    private List<VisualElement> _bulletElementList
        = new List<VisualElement>();

    public int BulletCount
    {
        get => _bulletCount;
        set
        {
            _bulletCount = Mathf.Clamp(value, 0, 7);
            ReDrawBullets();
        }
    }

    public BulletUI(VisualElement element, int bulletCount = 7)
    {
        _element        = element;
        _bulletCount    = bulletCount;

        _bulletElementList = element.Query<VisualElement>("bullet").ToList();
        _bulletElementList.ForEach(b => b.RemoveFromClassList("off"));
    }
    
    private void ReDrawBullets()
    {
        for(int i = 0; i < _bulletElementList.Count; i++)
        {
            if (_bulletElementList.Count - i > _bulletCount)
            {
                _bulletElementList[i].AddToClassList("off");
            }
            else
            {
                _bulletElementList[i].RemoveFromClassList("off");
            }
        }
    }

}
