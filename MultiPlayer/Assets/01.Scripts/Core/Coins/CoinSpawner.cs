using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [Header("���� ��")]
    [SerializeField] private RespawningCoin _coinPrefab;

    [Header("���� ��")]
    [SerializeField] private int _maxCoins = 30;
    [SerializeField] private int _coinValue = 10; //���δ� 10����
    [SerializeField] private LayerMask _whatIsObstacle;
    [SerializeField] private float _spawningTerm = 30f;
    [SerializeField] private float _spawningRadius = 8f;

    private bool _isSpawning = false;
    private float _spawningTime = 0;
    private int _spawnCountTime = 10; //10�� ī�����ϰ� ����

    public List<Transform> spawnPointList;  //������ ������ ������ ����Ʈ
    private float _coinRadius;

    private Stack<RespawningCoin> _coinPool = new Stack<RespawningCoin>(); //���� Ǯ
    private List<RespawningCoin> _activeCoinList = new List<RespawningCoin>(); //������ �����Ǹ� ���� ����Ʈ

    private RespawningCoin SpawnCoin()
    {
        var coin = Instantiate(_coinPrefab, Vector3.zero, Quaternion.identity);
        coin.SetValue(_coinValue);
        coin.GetComponent<NetworkObject>().Spawn(); //������ Ŭ��鿡�� ������ �˸�.
        coin.OnCollected += HandleCoinCollected;

        return coin;
    }

    private void HandleCoinCollected(RespawningCoin coin)
    {
        _activeCoinList.Remove(coin); //Ȱ��ȭ�� ���ο��� �ش� ������ ����
        coin.isActive.Value = false;
        coin.SetVisible(false);
        _coinPool.Push(coin); //���ÿ� �ٽ� �־��ְ�
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        _coinRadius = _coinPrefab.GetComponent<CircleCollider2D>().radius; //������ ì�ܵΰ�
        
        for(int i = 0; i < _maxCoins; i++)
        {
            var coin = SpawnCoin();
            //coin.isActive.Value = false;
            coin.SetVisible(false);
            _coinPool.Push(coin);
        }
    }

    public override void OnNetworkDespawn()
    {
        StopAllCoroutines(); //��� �ڷ�ƾ ����
    }


    #region ���λ�������
    private void Update()
    {
        if (!IsServer) return; //������ �ƴϸ� �ƿ� ������ �ʿ� ����.
        
        //���� �������� ���۵��� �ʾҰ� ������ ������ �ƹ��͵� ���ٸ� ���� ������ Ÿ�̹��� ��� ����
        if(!_isSpawning && _activeCoinList.Count == 0)
        {
            _spawningTime += Time.deltaTime;
            if(_spawningTime >= _spawningTerm)
            {
                _spawningTime = 0;
                StartCoroutine(SpawnCoroutine());
            }
        }
    }

    IEnumerator SpawnCoroutine()
    {
        _isSpawning = true;

        int pointIdx = Random.Range(0, spawnPointList.Count);
        int coinCount = Random.Range(_maxCoins / 2, _maxCoins + 1);

        Vector2 center = spawnPointList[pointIdx].position;
        for(int i = _spawnCountTime; i > 0; i--)
        {
            ServerCountDownMessageClientRpc(i, pointIdx, coinCount);
            yield return new WaitForSeconds(1f);
        }

        for(int i = 0; i < coinCount; i++)
        {
            Vector2 pos = Random.insideUnitCircle * _spawningRadius + center;
            var coin = _coinPool.Pop();
            coin.transform.position = pos;
            coin.Reset();
            _activeCoinList.Add(coin);
            yield return new WaitForSeconds(4f); //4�ʸ��� �Ѱ���
        }
        _isSpawning = false;
    }

    [ClientRpc]
    private void ServerCountDownMessageClientRpc(int sec, int pointIdx, int coinCount)
    {
        Debug.Log($"{pointIdx} �� �������� {sec}�� �� {coinCount} ���� ������ �����˴ϴ�.");
    }
    #endregion
}
