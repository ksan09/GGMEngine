using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavGameManager : MonoBehaviour
{
    NavMeshSurface _nevSurface;

    public static NavGameManager Instance;

    [SerializeField]
    private LayerMask _whatIsBase;

    private Camera _mainCam;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Multiple GameManager is running");
        }
        Instance = this;

        _mainCam = Camera.main; //메인 카메라 캐싱
        _nevSurface = GetComponent<NavMeshSurface>();
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool result = Physics.Raycast(ray, out hit, _mainCam.farClipPlane, _whatIsBase);
            if(result)
            {
                BaseBlock block = hit.collider.GetComponent<BaseBlock>();

                block?.ClickBaseBlock();
                ReBakeMesh();
            }
        }
    }

    private void ReBakeMesh()
    {
        _nevSurface.BuildNavMesh();

    }

    public bool GetMouseWorldPosition(out Vector3 pos)
    {
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool result = Physics.Raycast(ray, out hit, _mainCam.farClipPlane, _whatIsBase);
        if (result)
        {
            pos = hit.point;
            return true;
        }
        pos = Vector3.zero;
        return false;
    }
}
