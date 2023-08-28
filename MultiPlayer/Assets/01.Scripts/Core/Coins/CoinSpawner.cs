using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [Header("참조 값")]
    [SerializeField] private RespawningCoin _coinPrefab;

    [Header("셋팅 값")]
    [SerializeField] private int _maxCoins = 30;
    [SerializeField] private int _coinValue = 10; //코인당 10개씩
    [SerializeField] private LayerMask _whatIsObstacle;
    [SerializeField] private float _spawningTerm = 30f;
    [SerializeField] private float _spawningRadius = 8f;

    private bool _isSpawning = false;
    private float _spawningTime = 0;
    private int _spawnCountTime = 10; //10초 카운팅하고 시작

    public List<Transform> spawnPointList;  //코인이 스폰될 지점의 리스트
    private float _coinRadius;

    private Stack<RespawningCoin> _coinPool = new Stack<RespawningCoin>(); //코인 풀
    private List<RespawningCoin> _activeCoinList = new List<RespawningCoin>(); //코인이 스폰되면 들어올 리스트

    private RespawningCoin SpawnCoin()
    {
        var coin = Instantiate(_coinPrefab, Vector3.zero, Quaternion.identity);
        coin.SetValue(_coinValue);
        coin.GetComponent<NetworkObject>().Spawn(); //서버가 클라들에게 스폰을 알림.
        coin.OnCollected += HandleCoinCollected;

        return coin;
    }

    private void HandleCoinCollected(RespawningCoin coin)
    {
        _activeCoinList.Remove(coin); //활성화된 코인에서 해당 코인은 제거
        coin.isActive.Value = false;
        coin.SetVisible(false);
        _coinPool.Push(coin); //스택에 다시 넣어주고
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        _coinRadius = _coinPrefab.GetComponent<CircleCollider2D>().radius; //반지름 챙겨두고
        
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
        StopAllCoroutines(); //모든 코루틴 중지
    }


    #region 코인생성로직
    private void Update()
    {
        if (!IsServer) return; //서버가 아니면 아예 로직이 필요 없다.
        
        //현재 스포닝이 시작되지 않았고 생성된 코인이 아무것도 없다면 코인 스포닝 타이밍을 재기 시작
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
            yield return new WaitForSeconds(4f); //4초마다 한개씩
        }
        _isSpawning = false;
    }

    [ClientRpc]
    private void ServerCountDownMessageClientRpc(int sec, int pointIdx, int coinCount)
    {
        Debug.Log($"{pointIdx} 번 지점에서 {sec}초 후 {coinCount} 개의 코인이 생성됩니다.");
    }
    #endregion
}
