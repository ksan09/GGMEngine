using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnPoint : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer _minimapIcon;
    private Color _minimapIconColor;

    public string pointName;
    public Vector3 Position => transform.position;
    [field: SerializeField] public float Radius { get; private set; } = 10f;

    public List<Vector3> spawnPointList { get; private set; }

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            spawnPointList = MapManager.Instance.GetAvailablePositionList(Position, Radius);
        }
    }
    public void BlinkMinimapIcon()
    {
        _minimapIconColor = _minimapIcon.color;
        StartCoroutine("BlinkCo");
    }
    public void CloseMinimapIcon()
    {
        _minimapIconColor.a = 0;
        _minimapIcon.color = _minimapIconColor;
    }
    IEnumerator BlinkCo()
    {
        _minimapIconColor.a = 0;
        _minimapIcon.color = _minimapIconColor;
        float curTime = 0, endTime = 1;
        while(curTime < endTime)
        {
            curTime += Time.deltaTime;
            _minimapIconColor.a = curTime;
            _minimapIcon.color = _minimapIconColor;
            yield return null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.color = Color.white;
    }
#endif
}
