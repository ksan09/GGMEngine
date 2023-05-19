using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WobblyText : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _text.ForceMeshUpdate();
        TMP_TextInfo textInfo = _text.textInfo;

        for(int i = 0; i < textInfo.characterCount; ++i)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (charInfo.isVisible == false) continue;

            Vector3[] verties = textInfo.meshInfo[0].vertices;

            int vIndex0 = charInfo.vertexIndex;
            for(int j = 0; j < 4; ++j)
            {
                Vector3 origin = verties[vIndex0 + j];
                verties[vIndex0 + j] = origin + (Vector3.up * Mathf.Sin(Time.time*20) * 20);
            }
        }

        for(int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;

            _text.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
