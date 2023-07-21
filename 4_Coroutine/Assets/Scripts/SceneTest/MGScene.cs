using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * �̱��� Ŭ����
 * 
 */

public enum eSceneName
{
    None = -1,
    Loading,
    Logo,
    Title,
    Game,
}

public class MGScene : MonoBehaviour
{
    private static MGScene _instance;
    public static MGScene Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MGScene>() as MGScene;
                if(_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<MGScene>();
                }
            }
            return _instance;
        }
    }

    private StringBuilder _sb;

    private eSceneName _curScene;
    private Transform _uiRootTrm;       // ��夤 ui�� ���ʰ� �Ǵ� ĵ���� 
    private Transform _addUiTrm;        // Root�ؿ� �߰��Ǵ� �� ���� 1��1�� �����ϴ� UI

    // ui��麰 ������ ���� �뵵(�ϵ��ڵ�)
    public GameObject uiRootPrefab;

    public GameObject LoadingUIPrefab;
    public GameObject LogoUIPrefab;
    public GameObject GameUIPrefab;
    public GameObject TitleUIPrefab;

    public void Generate()              // �����ϴ� �뵵 
    {

    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _sb = new StringBuilder();

        GameObject uiRootObj = GameObject.Instantiate(uiRootPrefab) as GameObject;
        _uiRootTrm = uiRootObj.transform;

        InitScene();
    }

    private void InitScene()
    {
        //���ʷ� ȭ�鿡 ������ ���
        ChangeScene(eSceneName.Logo);
    }

    public void ChangeScene(eSceneName inScene)
    {
        _curScene = inScene;

        _sb.Remove(0, _sb.Length);
        _sb.AppendFormat("{0}Scene", eSceneName.Loading);
        SceneManager.LoadScene(_sb.ToString());

        ChangeUI(eSceneName.Loading);
    }

    private void ChangeUI(eSceneName inScene)
    {
        // ���� ui �������� �ı�
        if(_addUiTrm != null)
        {
            _addUiTrm.SetParent(null);
            GameObject.Destroy(_addUiTrm.gameObject);
        }

        // eSceneName�� �ϴ��Ϸ� ��Ī�Ǵ� ui���� �ε�
        // �ϵ��ڵ�
        GameObject go = null;

        if (inScene == eSceneName.Loading)
            go = GameObject.Instantiate(LoadingUIPrefab) as GameObject;
        else if (inScene == eSceneName.Logo)
            go = GameObject.Instantiate(LogoUIPrefab) as GameObject;
        else if (inScene == eSceneName.Title)
            go = GameObject.Instantiate(TitleUIPrefab) as GameObject;
        else if (inScene == eSceneName.Game)
            go = GameObject.Instantiate(GameUIPrefab) as GameObject;

        _addUiTrm = go.transform;
        _addUiTrm.SetParent(_uiRootTrm);

        _addUiTrm.localPosition = Vector3.zero;
        _addUiTrm.localScale    = Vector3.one;

        RectTransform rts = go.GetComponent<RectTransform>();
        rts.offsetMax = Vector2.zero;
        rts.offsetMin = Vector2.zero;

    }

    public void LoadingDone()
    {
        ChangeUI(_curScene);

        Debug.Log("�ε� ��");
    }
}
