using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class MultiLogManager : MonoSingleton<MultiLogManager>
{
    private string _nameCache = "";
    private bool _isSetted = false;

    private void Awake()
    {
        ServerManager.Instance.OnJoin += OnJoin;
        ServerManager.Instance.OnLeave += OnLeave;
        ServerManager.Instance.OnLeaderboard += OnLeaderboard;
    }

    private void OnJoin(ServerManager.JoinPacket joinPacket)
    {
        if (!_isSetted)
            return;

        Log($"{joinPacket.Name}���� �����ϼ̽��ϴ�.");
    }

    private void OnLeave(ServerManager.LeavePacket leavePacket)
    {
        Log($"{RealtimeLeaderboardManager.Instance.GetNameById(leavePacket.Id)}���� ������ �����̽��ϴ�.");
    }

    private void OnLeaderboard(ServerManager.RealtimeLeaderboardPacket leaderboardPacket)
    {
        if (!_isSetted)
        {
            Debug.Log($"Ignore leaderboard packet");
            _isSetted = true;
            return;
        }
        if (RealtimeLeaderboardManager.Instance.GetFirstEntry() == null) return;
        if (_nameCache != RealtimeLeaderboardManager.Instance.GetFirstEntry().Name)
        {
            Log($"{RealtimeLeaderboardManager.Instance.GetFirstEntry().Name}���� ���θ� �޸��� �ֽ��ϴ�.");
            _nameCache = RealtimeLeaderboardManager.Instance.GetFirstEntry().Name;
        }
    }

    public void Log(string message)
    {
        foreach (var entry in PoolManager<MultiLogEntry>.GetAllActive())
        {
            entry.EntryCreated();
        }

        var enrtyObject = PoolManager<MultiLogEntry>.Get(transform);
        Debug.Log($"{enrtyObject.name}");
        enrtyObject.MessageText.text = message;
        (enrtyObject.transform as RectTransform).anchoredPosition = new Vector2(-500, 0);
    }
}
