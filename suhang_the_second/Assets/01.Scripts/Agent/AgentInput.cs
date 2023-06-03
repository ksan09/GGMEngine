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
        float horizontal = Input.GetAxisRaw("Horizontal"); //�̰� Y�� �������� �ȴ�.
        float vertical = Input.GetAxisRaw("Vertical"); //�̰� Z��������� �ǰ�
        _movement = new Vector3(horizontal, 0, vertical);
        OnMovementKeyPress?.Invoke(_movement);
    }

    public bool GetMouseWorldPosition(out Vector3 point)
    {
        Ray ray = Define.MainCam.ScreenPointToRay(Input.mousePosition);
        //��ũ���� �ִ� ���콺�� ��ġ�� ���ϴ� Ray�� �����.
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
