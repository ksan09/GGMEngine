using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    [SerializeField][Range(0.5f, 3f)]
    private float _casterRadius = 1f;
    [SerializeField]
    private float _casterInterpolation = 0.5f;
    [SerializeField]
    private int _casterDamage = 20;

    [SerializeField]
    private LayerMask _targetLayer;

    public void CastDamage()
    {
        Vector3 startPos = transform.position + transform.forward * -_casterRadius; //반지름만큼 뒤로 빼서
        RaycastHit hit;
        bool isHit = Physics.SphereCast(startPos, 
            _casterRadius, 
            transform.forward, 
            out hit, 
            _casterRadius + _casterInterpolation,
            _targetLayer);

        if(isHit)
        {
            if (hit.collider.TryGetComponent<IDamageable>(out IDamageable health))
            {
                health.OnDamage(_casterDamage, hit.point, hit.normal);
            }            
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(UnityEditor.Selection.activeGameObject == gameObject)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _casterRadius);
            Gizmos.color = Color.white;
        }
    }
#endif
}
