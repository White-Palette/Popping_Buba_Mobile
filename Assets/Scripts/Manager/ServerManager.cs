using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using System;
using DG.Tweening;

public class ServerManager : MonoSingleton<ServerManager>
{
    public class Packet
    {
        [JsonProperty("t")]
        public string Type;
        [JsonProperty("p")]
        public string Payload;

        public Packet(string type, string payload)
        {
            Type = type;
            Payload = payload;
        }
    }

    public class RealtimeLeaderboardEntry
    {
        [JsonProperty("i")]
        public int Id;
        [JsonProperty("h")]
        public float Height;
        [JsonProperty("c")]
        public int Combo;

        public RealtimeLeaderboardEntry(int id, float height, int combo)
        {
            Id = id;
            Height = height;
            Combo = combo;
        }
    }

    public class RealtimeLeaderboardPacket
    {
        [JsonProperty("l")]
        public List<RealtimeLeaderboardEntry> Leaderboard;

        public RealtimeLeaderboardPacket(List<RealtimeLeaderboardEntry> leaderboard)
        {
            Leaderboard = leaderboard;
        }
    }

    private WebSocket ws;

    public class JoinPacket
    {
        [JsonProperty("i")]
        public int Id;
        [JsonProperty("n")]
        public string Name;
        [JsonProperty("c")]
        public string Color;
    }

    public class LeavePacket
    {
        [JsonProperty("i")]
        public int Id;
    }

    private Queue<Action> taskQueue = new Queue<Action>();

    public Action OnConnected;
    public Action<JoinPacket> OnJoin;
    public Action<LeavePacket> OnLeave;
    public Action<RealtimeLeaderboardPacket> OnLeaderboard;

    private void Awake()
    {
        ws = new WebSocket("ws://141.164.53.243:3002/");
        ws.EmitOnPing = true;

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Connected");
            SendName(UserData.UserName, UserData.Color);
            taskQueue.Enqueue(() =>
            {
                OnConnected?.Invoke();
            });
        };
        ws.OnMessage += (sender, e) =>
        {
            try
            {
                if (e.Data == "" || e.Data == null)
                    return;

                Packet packet = JsonConvert.DeserializeObject<Packet>(e.Data);
                switch (packet.Type)
                {
                    case "i":
                        Debug.Log("Id: " + packet.Payload);
                        break;
                    case "li":
                        RealtimeLeaderboardPacket leaderboardPacket = JsonConvert.DeserializeObject<RealtimeLeaderboardPacket>(packet.Payload);
                        taskQueue.Enqueue(() =>
                        {
                            OnLeaderboard?.Invoke(leaderboardPacket);
                        });
                        break;
                    case "j":
                        JoinPacket joinPacket = JsonConvert.DeserializeObject<JoinPacket>(packet.Payload);
                        taskQueue.Enqueue(() =>
                        {
                            OnJoin?.Invoke(joinPacket);
                        });
                        break;
                    case "l":
                        LeavePacket leavePacket = JsonConvert.DeserializeObject<LeavePacket>(packet.Payload);
                        taskQueue.Enqueue(() =>
                        {
                            OnLeave?.Invoke(leavePacket);
                        });
                        break;
                    default:
                        Debug.Log($"Unknown packet type: {packet.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        };
        ws.OnClose += (sender, e) =>
        {
            Debug.Log("Closed");
        };
        ws.OnError += (sender, e) =>
        {
            Debug.Log("Error: " + e.Message);
        };
        ws.Connect();
    }

    float heightCache = 0;
    float comboCache = 0;

    public void SendHeight(float height, int combo)
    {
        if (height == heightCache && combo == comboCache)
            return;
        ws.Send(JsonConvert.SerializeObject(new Packet("h", JsonConvert.SerializeObject(new { h = height, c = combo }))));
        heightCache = height;
        comboCache = combo;
    }

    public void SendName(string name, Color color)
    {
        string hex = ColorUtility.ToHtmlStringRGB(color);
        ws.Send(JsonConvert.SerializeObject(new Packet("n", JsonConvert.SerializeObject(new { n = name, c = hex }))));
    }

    private void OnDestroy()
    {
        ws.Close();
    }

    private void Update()
    {
        if (taskQueue.Count > 0)
        {
            taskQueue.Dequeue()();
        }
    }
}
