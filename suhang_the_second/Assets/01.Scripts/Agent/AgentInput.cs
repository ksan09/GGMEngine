using Core;
using UnityEngine;
using UnityEngine.Events;

public class AgentInput : MonoBehaviour
{
    [SerializeField]
    private LayerMask _whatIsGround;

    private Vector3 _movement;

    public UnityEvent<Vector3> OnMovementKeyPress = null;
    public UnityEvent OnAttackKeyPress = null;
    public UnityEvent<Vector3> OnPointerPositionChanged = null;
    
    private void Update()
    {
        UpdateMoveInput();
        UpdateAttackInput();
        UpdateRotateInput();
    }

    private void UpdateRotateInput()
    {
        if(GetMouseWorldPosition(out Vector3 point))
        {
            OnPointerPositionChanged?.Invoke(point);
        }  
    }

    private void UpdateAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnAttackKeyPress?.Invoke(); 
        }
    }

    private void UpdateMoveInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); //이게 Y축 움직임이 된다.
        float vertical = Input.GetAxisRaw("Vertical"); //이게 Z축움직임이 되고
        _movement = new Vector3(horizontal, 0, vertical);
        OnMovementKeyPress?.Invoke(_movement);
    }

    public bool GetMouseWorldPosition(out Vector3 point)
    {
        Ray ray = Define.MainCam.ScreenPointToRay(Input.mousePosition);
        //스크린에 있는 마우스의 위치를 향하는 Ray를 만든다.
        RaycastHit hit;

        bool result = Physics.Raycast(ray, out hit, Define.MainCam.farClipPlane, _whatIsGround);
        if (result)
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                point = hit.collider.transform.position;
            }
            else
            {
                point = hit.point;
            }
        }
        else
            point = Vector3.zero;

        return result;

    }

    public Vector3 GetCurrentInput()
    {
        return _movement;
    }
}
