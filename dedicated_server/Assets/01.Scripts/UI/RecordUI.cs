using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class RecordUI : MonoBehaviour, IComparer<RecordUI>
{
    private TextMeshProUGUI _recordText;

    public ulong clientID;
    public string username;
    public int score;

    private void Awake()
    {
        _recordText = GetComponent<TextMeshProUGUI>();

    }

    public void SetOwner(ulong ownerID)
    {
        clientID = ownerID;
    }

    public void SetText(int rank, string username, int score)
    {
        this.username = username;
        this.score = score;
        _recordText.SetText($"{rank.ToString()} . {username} [ {score.ToString()} ]");
    }

    public int Compare(RecordUI x, RecordUI y)
    {
        if (x.score > y.score)
            return -1;
        else if (x.score == y.score)
            return 0;
        else
            return 1;
    }
}
