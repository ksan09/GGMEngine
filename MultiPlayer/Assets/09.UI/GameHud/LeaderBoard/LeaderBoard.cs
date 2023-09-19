using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            item.Root.RemoveFromHierarchy(); //�̰� �ϸ� UI���� �����
            _itemList.Remove(item);
        }
    }

    public void UpdateByClientID(ulong clientID, int coins)
    {
        //������ Ŭ���̾�Ʈ ���̵��� state�� ã�Ƽ� coin�� ������Ʈ ��Ŷ��.
        BoardItem item = _itemList.FirstOrDefault(x => x.ClientID == clientID);
        if(item != null)
        {
            item.UpdateCoin(coins);
        }
    }

    public void SortOrder()
    {
        //�̰� �����ð�

        _itemList.OrderBy(x => x.Coins);

        _innerHolder.Clear();
        //for(int i = 0; i < _itemList.Count; i++)
        //    for(int j = 0; j < _itemList.Count - 1; ++j)
        //        if (_itemList[j].Coins < _itemList[j + 1].Coins)
        //        {
        //            BoardItem temp = _itemList[j];
        //            _itemList[j] = _itemList[j + 1];
        //            _itemList[j + 1] = temp;
        //        }

        for (int i = 0; i < _itemList.Count; ++i)
            _innerHolder.Add(_itemList[i].Root);
    }
}
