using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> _keys = new List<TKey>();
    [SerializeField] private List<TValue> _values = new List<TValue>();



    // 시리얼라이즈된 것을 읽을 때 할 일
    public void OnAfterDeserialize()
    {
        this.Clear();

        if(_keys.Count != _values.Count)
        {
            Debug.LogError("[Key, Count] doesn't match to value count");
            return;
        }

        for(int i = 0; i < _keys.Count; i++)
        {
            this.Add(_keys[i], _values[i]);
        }
    }

    // 시리얼라이즈 하기 전에 해줄 일
    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var pair in this)
        {
            _keys.Add(pair.Key);
            _values.Add(pair.Value);
        }
    }
}
