using Unity.Netcode;
using UnityEngine;

public class ShootKnife : NetworkBehaviour
{
    [Header("참조변수들")]
    [SerializeField] private InputReader    _inputReader;
    [SerializeField] private Transform      _shootPositionTrm; // 이거의 right 방향으로 슈웃
    // 서버용 프리팹
    [SerializeField] private GameObject _serverKnifePrefab;
    // 클라이언트용 프리팹
    [SerializeField] private GameObject _clientKnifePrefab;
    // 자기 자신과 충돌방지를 위한 플레이어 콜라이더도 필요하다
    [SerializeField] private Collider2D _playerCollider;

    [Header("셋팅값들")]
    [SerializeField] private float  _knifeSpeed;
    [SerializeField] private int    _knifeDamage;

    [SerializeField] private float  _throwCooltime;
    private float _lastThrowTime;

    // 자기 자신만 inputReader에서 ShootEvent를 구독해야한다.
    // 해지도 잘 해야된다

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _inputReader.ShootEvent += HandleShootKnife;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        _inputReader.ShootEvent -= HandleShootKnife;
    }

    private void HandleShootKnife()
    {
        if (Time.time < _lastThrowTime + _throwCooltime)
            return;

        _lastThrowTime = Time.time;

        //쿨타임이 돌아왔다면 발사 하는거야
        //클라꺼 쏘고
        ShotKnife(_clientKnifePrefab);
        //서버 RPC날리고
        //서버 RPC는 클라 RPC날리고
        //자기자신은 안만들어주고 나머지 클라는 만든다.
        SpawnKnifeServerRPC();
        //모든 나이프는 만들때 
        //Physics2D.IgnoreCollision 을 이용해서 자기자신과는 충돌하지 않게 만든다.
    }

    [ServerRpc]
    public void SpawnKnifeServerRPC()
    {
        //
        UserData data = ServerSingleton.Instance.NetServer.GetUserDataByClientID(OwnerClientId);
        ShotKnife(_serverKnifePrefab);
        SpawnDummyKnifeClientRPC();
    }

    [ClientRpc]
    public void SpawnDummyKnifeClientRPC()
    {
        if (IsOwner) return;
        ShotKnife(_clientKnifePrefab);
    }

    private void ShotKnife(GameObject obj)
    {
        GameObject instance = Instantiate(obj, _shootPositionTrm.position, Quaternion.identity);
        instance.transform.right = _shootPositionTrm.right;

        if(instance.TryGetComponent<Collider2D>(out Collider2D col))
        {
            Physics2D.IgnoreCollision(col, _playerCollider);
        }

        if(instance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = _shootPositionTrm.right * _knifeSpeed;
        }

        if(instance.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact damage))
        {
            damage.SetDamage(_knifeDamage);
            damage.SetOwner(OwnerClientId);
        }
        
    }
}
