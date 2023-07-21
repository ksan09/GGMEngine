using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 싱글톤 클래스
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
    private Transform _uiRootTrm;       // 모드ㄴ ui의 기초가 되는 캔버스 
    private Transform _addUiTrm;        // Root밑에 추가되는 각 씬과 1대1로 존재하는 UI

    // ui장면별 프리팹 연결 용도(하드코딩)
    public GameObject uiRootPrefab;

    public GameObject LoadingUIPrefab;
    public GameObject LogoUIPrefab;
    public GameObject GameUIPrefab;
    public GameObject TitleUIPrefab;

    public void Generate()              // 생성하는 용도 
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
        //최초로 화면에 보여줄 장면
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
        // 기존 ui 프리팹은 파괴
        if(_addUiTrm != null)
        {
            _addUiTrm.SetParent(null);
            GameObject.Destroy(_addUiTrm.gameObject);
        }

        // eSceneName과 일대일로 매칭되는 ui들을 로드
        // 하드코딩
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

        Debug.Log("로딩 끝");
    }
}
