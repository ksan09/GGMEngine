using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class LeaderBoard
{
    private VisualElement _root;
    private int _displayCount;
    private VisualElement _innerHolder;

    private VisualTreeAsset _boardItemAsset;
    private List<BoardItem> _itemList = new List<BoardItem>();

    private Color _ownerColor;

    public LeaderBoard(VisualElement root, VisualTreeAsset itemAsset, Color ownerColor,
                        int displayCount = 7)
    {
        _root = root;
        _innerHolder = _root.Q<VisualElement>("inner-holder"); 

        _boardItemAsset = itemAsset;
        _displayCount = displayCount;
        _ownerColor = ownerColor;
    }

    public bool CheckExistByClientID(ulong clientID)
    {
        return _itemList.Any(x => x.ClientID == clientID);
    }

    public void AddItem(LeaderboardEntityState state)
    {
        var root = _boardItemAsset.Instantiate().Q<VisualElement>("board-item");
        _innerHolder.Add(root);
        BoardItem item = new BoardItem(root, state, _ownerColor);
        _itemList.Add(item);
    }

    public void RemoveByClientID(ulong clientID)
    {
        BoardItem item = _itemList.FirstOrDefault(x => x.ClientID == clientID);

        if(item != null)
        {
            item.Root.RemoveFromHierarchy(); //이거 하면 UI에서 사라짐
            _itemList.Remove(item);
        }
    }

    public void UpdateByClientID(ulong clientID, int coins)
    {
        //지정된 클라이언트 아이디의 state를 찾아서 coin을 업데이트 시킷오.
        BoardItem item = _itemList.FirstOrDefault(x => x.ClientID == clientID);
        if(item != null)
        {
            item.UpdateCoin(coins);
        }
    }

    public void SortOrder()
    {
        //이거 다음시간
        //b-a 내림차, a-b 오름차
        _itemList.Sort((a, b) => b.Coins.CompareTo(a.Coins)); // 이건 내림차순
        for(int i = 0; i < _itemList.Count; ++i)
        {
            var item = _itemList[i];
            item.rank = i + 1; // 등수 기록
            item.Root.BringToFront();
            item.UpdateText();

            item.Show(i < _displayCount); // 표현해야할 수보다 작으면 표시

            //자기꺼는 무조건 표시
            if(item.ClientID == NetworkManager.Singleton.LocalClientId)
            {
                item.Show(true);
            }
        }
    }
}
