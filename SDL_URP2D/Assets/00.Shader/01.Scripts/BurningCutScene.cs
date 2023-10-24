using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningCutScene : MonoBehaviour
{
    [SerializeField] Material _shaderMat;

    [SerializeField] private float startV;
    [SerializeField] private float endV;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(FadeCo());
        }
    }

    IEnumerator FadeCo()
    {
        float currentTime = 0f;
        float endTime = 2f;
        while(currentTime < endTime)
        {
            _shaderMat.SetFloat("_ShowValue", currentTime / endTime * 1.05f);
            currentTime += Time.deltaTime;
            yield return null;
        }
        _shaderMat.SetFloat("_ShowValue", 1.05f);

    }
}
