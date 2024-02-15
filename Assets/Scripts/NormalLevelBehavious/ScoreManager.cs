using UnityEngine;
using UnityEngine.UI; // Make sure to include this if you're planning to update UI elements like text for scores.

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public Text scoreText; // Assume you have a Text UI element for displaying the score.
    private int score = 0;

    void Awake()
    {
        // Ensure there's only one instance of the ScoreManager running.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int pointsToAdd)
    {
        score += pointsToAdd;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    // Utility method for other scripts to get the current score.
    public int GetCurrentScore()
    {
        return score;
    }
}
