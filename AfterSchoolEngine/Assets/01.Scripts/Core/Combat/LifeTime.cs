using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [SerializeField] private float _lifetime;
    private float _currentTime = 0;

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if(_currentTime >= _lifetime)
        {
            Destroy(gameObject);
            // 만약 풀링한다면 클라이언트에서만 풀링할 것.
        }
    }
}
