using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerShaderController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Material _material;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("ShowCo");
        }
    }

    private readonly int _hashMatShowValue = Shader.PropertyToID("_ShowValue");
    IEnumerator ShowCo()
    {
        float time = 0.5f;
        float currentTime = 0f;
        float percent = 0, value;
        while(percent < 1)
        {
            percent = currentTime / time;
            value = Mathf.Lerp(1, -1, percent);
            _spriteRenderer.material.SetFloat(_hashMatShowValue, value);

            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
