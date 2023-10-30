using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShockWaveBlock : MonoBehaviour
{
    [SerializeField]
    private float _shockWaveTime = 0.5f;
    [SerializeField]
    private float _delayTime = 0.1f;

    private Material _shockWaveShaderMat;
    private const string matChangeValueName = "_WaveDistanceFromCenter";

    private void Awake()
    {
        _shockWaveShaderMat = GetComponent<SpriteRenderer>().material;

        StartCoroutine(DoShockWaveLoop());
    }

    
    
    IEnumerator DoShockWaveLoop()
    {
        WaitForSeconds _delay = new WaitForSeconds(_delayTime);
        float currentTime;

        while(true)
        {
            currentTime = 0f;
            while(currentTime <= _shockWaveTime)
            {
                currentTime += Time.deltaTime;
                _shockWaveShaderMat.SetFloat(matChangeValueName,
                    Mathf.Lerp(0f, 1f, currentTime / _shockWaveTime));
                yield return null;
            }
            yield return _delay;
        }
    }

}
