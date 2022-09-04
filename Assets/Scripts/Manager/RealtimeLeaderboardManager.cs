using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RealtimeLeaderboardManager : MonoSingleton<RealtimeLeaderboardManager>
{
    [SerializeField] Transform realtimeLeaderboardTransform = null;

    private Dictionary<int, RealtimeLeaderboardEntry> realtimeLeaderboard = new Dictionary<int, RealtimeLeaderboardEntry>();

    private bool isFireLog = false;

    private void Awake()
    {
        ServerManager.Instance.OnJoin += OnJoin;

        ServerManager.Instance.OnLeave += OnLeave;

        ServerManager.Instance.OnLeaderboard += OnLeaderboard;
    }

    private void OnJoin(ServerManager.JoinPacket packet)
    {
        var leaderboardEntry = PoolManager<RealtimeLeaderboardEntry>.Get(realtimeLeaderboardTransform);
        leaderboardEntry.Name = packet.Name;
        leaderboardEntry.Height = 0;
        ColorUtility.TryParseHtmlString(packet.Color, out Color color);
        leaderboardEntry.Color = color;
        PlayerController.Instance.TrailColor(color);
        Debug.Log($"Color: {packet.Color}");
        realtimeLeaderboard.Add(packet.Id, leaderboardEntry);
    }

    private void OnLeave(ServerManager.LeavePacket packet)
    {
        if (realtimeLeaderboard.ContainsKey(packet.Id))
        {
            StartCoroutine(Leave(packet.Id));
        }
    }

    private void OnLeaderboard(ServerManager.RealtimeLeaderboardPacket packet)
    {
        float maxHeight = 0;
        float transformHeight = (realtimeLeaderboardTransform as RectTransform).rect.height;

        foreach (var entry in packet.Leaderboard)
        {
            if (realtimeLeaderboard.ContainsKey(entry.Id))
            {
                realtimeLeaderboard[entry.Id].Height = entry.Height;
                if (entry.Height > maxHeight)
                {
                    maxHeight = entry.Height;
                }
            }
        }

        float scale = transformHeight / maxHeight;
        if (maxHeight <= 0)
        {
            scale = 0;
        }

        foreach (var entry in packet.Leaderboard)
        {
            if (realtimeLeaderboard.ContainsKey(entry.Id))
            {
                realtimeLeaderboard[entry.Id].Height = entry.Height;
                DOTween.Kill(realtimeLeaderboard[entry.Id].transform);
                if ((realtimeLeaderboard[entry.Id].transform as RectTransform).anchoredPosition.y == entry.Height * scale)
                    continue;
                (realtimeLeaderboard[entry.Id].transform as RectTransform).DOAnchorPosY(entry.Height * scale, 0.5f);

                if (entry.Combo >= 50)
                {
                    if (realtimeLeaderboard[entry.Id].IsFired) return;
                    MultiLogManager.Instance.Log($"<color=#e69019>{realtimeLeaderboard[entry.Id].Name}님이 불타고 있습니다.</color>");
                    realtimeLeaderboard[entry.Id].IsFired = true;
                }
                else
                {
                    realtimeLeaderboard[entry.Id].IsFired = false;
                }

                
            }
        }
    }

    private IEnumerator Leave(int id)
    {
        (realtimeLeaderboard[id].transform as RectTransform).DOAnchorPosY(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PoolManager<RealtimeLeaderboardEntry>.Release(realtimeLeaderboard[id]);
        realtimeLeaderboard.Remove(id);
    }

    public RealtimeLeaderboardEntry GetCurrentEntry()
    {
        return realtimeLeaderboard.Values.FirstOrDefault();
    }

    public RealtimeLeaderboardEntry GetFirstEntry()
    {
        return realtimeLeaderboard.Select(x => x.Value).OrderBy(x => x.Height).LastOrDefault();
    }

    public string GetNameById(int id)
    {
        if (realtimeLeaderboard.ContainsKey(id))
        {
            return realtimeLeaderboard[id].Name;
        }
        return "";
    }
}