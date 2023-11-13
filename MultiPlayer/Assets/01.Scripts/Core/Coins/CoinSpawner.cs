using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [Header("���� ��")]
    [SerializeField] private RespawningCoin _coinPrefab;
    [SerializeField] private DecalCircle _decalCircle;

    [Header("���� ��")]
    [SerializeField] private int _maxCoins = 30;
    [SerializeField] private int _coinValue = 10; //���δ� 10����
    [SerializeField] private float _spawningTerm = 30f;

    private bool _isSpawning = false;
    private float _spawningTime = 0;
    private int _spawnCountTime = 10; //10�� ī�����ϰ� ����

    public List<SpawnPoint> spawnPointList;  //������ ������ ������ ����Ʈ
    private float _coinRadius;

    private Stack<RespawningCoin> _coinPool = new Stack<RespawningCoin>(); //���� Ǯ
    private List<RespawningCoin> _activeCoinList = new List<RespawningCoin>(); //������ �����Ǹ� ���� ����Ʈ

    private bool _startGame = false;

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
            coin.isActive.Value = false;
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
        if (!_startGame) return;

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
        var point = spawnPointList[pointIdx];
        int maxCoinCount = Mathf.Min(_maxCoins + 1, point.spawnPointList.Count);
        int coinCount = Random.Range(maxCoinCount / 2, maxCoinCount);

        for(int i = _spawnCountTime; i > 0; i--)
        {
            ServerCountDownMessageClientRpc(i, pointIdx, coinCount);
            yield return new WaitForSeconds(1f);
        }
        CloseServerCountDownMessageClientRPC();

        for(int i = 0; i < coinCount; i++)
        {
            int end = point.spawnPointList.Count - i - 1;
            int idx = Random.Range(0, end + 1);

            Vector2 pos = point.spawnPointList[idx];

            (point.spawnPointList[idx], point.spawnPointList[end])
                = (point.spawnPointList[end], point.spawnPointList[idx]);

            var coin = _coinPool.Pop();
            coin.transform.position = pos;
            coin.Reset();
            _activeCoinList.Add(coin);
            yield return new WaitForSeconds(_spawningTerm); //term���� �Ѱ���
        }
        _isSpawning = false;
        CloseDecalCircleClientRPC(pointIdx); //����
        
    }

    [ClientRpc]
    private void CloseDecalCircleClientRPC(int pointIdx)
    {
        _decalCircle.CloseCircle();
        spawnPointList[pointIdx].CloseMinimapIcon();
    }

    [ClientRpc]
    private void ServerCountDownMessageClientRpc(int sec, int pointIdx, int coinCount)
    {
        var point = spawnPointList[pointIdx];
        if(!_decalCircle.showDecal) //ó�� �����̴� �� �� ��Į ��Ŭ ����
        {
            _decalCircle.OpenCircle(point.Position, point.Radius);
        }
        point.BlinkMinimapIcon();
        UIManager.Instance.ShowMsg(true, 
            $"{point.pointName} �������� {sec}�� �� {coinCount} ���� ������ �����˴ϴ�.");
    }

    [ClientRpc]
    private void CloseServerCountDownMessageClientRPC()
    {
        UIManager.Instance.ShowMsg(false);
    }

    public void StartSpawn()
    {
        _startGame = true;
    }
    #endregion
}
