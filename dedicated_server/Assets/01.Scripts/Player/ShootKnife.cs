using Unity.Netcode;
using UnityEngine;

public class ShootKnife : NetworkBehaviour
{
    [Header("����������")]
    [SerializeField] private InputReader    _inputReader;
    [SerializeField] private Transform      _shootPositionTrm; // �̰��� right �������� ����
    // ������ ������
    [SerializeField] private GameObject _serverKnifePrefab;
    // Ŭ���̾�Ʈ�� ������
    [SerializeField] private GameObject _clientKnifePrefab;
    // �ڱ� �ڽŰ� �浹������ ���� �÷��̾� �ݶ��̴��� �ʿ��ϴ�
    [SerializeField] private Collider2D _playerCollider;

    [Header("���ð���")]
    [SerializeField] private float  _knifeSpeed;
    [SerializeField] private int    _knifeDamage;

    [SerializeField] private float  _throwCooltime;
    private float _lastThrowTime;

    // �ڱ� �ڽŸ� inputReader���� ShootEvent�� �����ؾ��Ѵ�.
    // ������ �� �ؾߵȴ�

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

        //��Ÿ���� ���ƿԴٸ� �߻� �ϴ°ž�
        //Ŭ�� ���
        ShotKnife(_clientKnifePrefab);
        //���� RPC������
        //���� RPC�� Ŭ�� RPC������
        //�ڱ��ڽ��� �ȸ�����ְ� ������ Ŭ��� �����.
        SpawnKnifeServerRPC();
        //��� �������� ���鶧 
        //Physics2D.IgnoreCollision �� �̿��ؼ� �ڱ��ڽŰ��� �浹���� �ʰ� �����.
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
