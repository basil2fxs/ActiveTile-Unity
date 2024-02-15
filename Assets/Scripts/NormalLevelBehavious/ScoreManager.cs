using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int pointsToAdd = 100;
    public int pointsToTake = 50;
    public Text mainMenuScoreText; // Use in the main menu scene
    public Text gameScoreText; // Use in the game scene
    public Button resetScoreButton;
    private int score = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Attempt to find the Text components again in the newly loaded scene
        mainMenuScoreText = GameObject.FindWithTag("MainMenuScoreText")?.GetComponent<Text>();
        gameScoreText = GameObject.FindWithTag("GameScoreText")?.GetComponent<Text>();

        // Update the score UI to reflect current score
        UpdateScoreUI();
    }

    public void AddPoints(bool addOrTake)
    {
        if (!(GameManager.instance.IsInSafeSpace))
        {
            if(addOrTake == true)
            {
                score += pointsToAdd;
            }
            else
            {
                score -= pointsToTake;
            }
            UpdateScoreUI();    
        }
    }
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }


    public void UpdateScoreUI()
    {
        if (mainMenuScoreText != null) mainMenuScoreText.text = "End Score: " + score;
        if (gameScoreText != null) gameScoreText.text = "Score: " + score;
    }

    public int GetCurrentScore()
    {
        return score;
    }
}
