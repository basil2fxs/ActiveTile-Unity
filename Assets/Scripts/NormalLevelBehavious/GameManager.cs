using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable]
public class LeaderboardEntry
{
    public string teamName;
    public int score;

    public LeaderboardEntry(string name, int score)
    {
        this.teamName = name;
        this.score = score;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

    private string leaderboardFilePath;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        leaderboardFilePath = Application.persistentDataPath + "/leaderboard.json";
        LoadLeaderboard();
    }

    public void AddScore(string teamName, int score)
    {
        // Update score for the current session
        // Check if we need to update an existing leaderboard entry
        var entry = leaderboard.FirstOrDefault(e => e.teamName == teamName);
        if (entry != null)
        {
            entry.score += score;
        }
        else
        {
            leaderboard.Add(new LeaderboardEntry(teamName, score));
        }
        SaveLeaderboard();
    }

    void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(new { leaderboard = this.leaderboard }, true);
        File.WriteAllText(leaderboardFilePath, json);
        Debug.Log("Leaderboard saved");
    }

    void LoadLeaderboard()
    {
        if (File.Exists(leaderboardFilePath))
        {
            string json = File.ReadAllText(leaderboardFilePath);
            this.leaderboard = JsonUtility.FromJson<List<LeaderboardEntry>>(json);
            Debug.Log("Leaderboard loaded");
        }
    }

    // Add other GameManager methods here...
}
