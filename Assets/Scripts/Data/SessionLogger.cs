using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LogEvent
{
    public string timestamp;
    public string playerId;
    public string action;
    public string target;
    public string puzzleState;
}

public class SessionLogger : MonoBehaviour
{
    public static SessionLogger Instance;
    private List<LogEvent> events = new List<LogEvent>();
    private string sessionId;

    void Awake()
    {
        Instance = this;
        sessionId = Guid.NewGuid().ToString().Substring(0, 8);
    }

    public void Log(string playerId, string action, string target = "")
    {
        var pm = PuzzleManager.Instance;
        string state = pm != null
            ? $"P1:{pm.puzzle1State.Value} P2:{pm.puzzle2State.Value} P3:{pm.puzzle3Complete.Value}"
            : "unknown";
        events.Add(new LogEvent
        {
            timestamp = DateTime.Now.ToString("HH:mm:ss.fff"),
            playerId = playerId,
            action = action,
            target = target,
            puzzleState = state
        });
    }

    public void SaveSession()
    {
        string json = JsonUtility.ToJson(new { sessionId, events }, true);
        string path = Application.persistentDataPath + $"/session_{sessionId}.json";
        File.WriteAllText(path, json);
        Debug.Log($"Session sauvegardee : {path}");
    }
}