using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int lives = 3; // Default number of lives
    private bool isInSafeSpace = true; // Flag to indicate if the player is in a safe space
     public float lifeCooldown = 2f; // Cooldown in seconds before a life can be deducted again
     private float lastLifeTime = -2f;

    // Public property to access the safe space state
    public bool IsInSafeSpace
    {
        get { return isInSafeSpace; }
        private set { isInSafeSpace = value; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the GameManager across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Ensure only one instance of the GameManager exists
        }

        ResetLives(); // Reset lives every time the scene is loaded
    }

    public void ResetLives()
    {
        lives = 3; // Default lives value
        // Attempt to update the UI if UIManager is available
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateLivesCounter(lives);
        }
    }
    public void DeductLife()
    {
        // Only deduct a life if not in a safe space
        if (!isInSafeSpace)
        {
            ScoreManager.instance?.AddPoints(false);
            if (Time.time - lastLifeTime >= lifeCooldown)
            {
                lastLifeTime = Time.time;

                // Deduct life logic here
                lives--;
                // Add any UI update logic here, if needed.
                if (lives <= 0)
                {
                    // Handle game over logic here
                    Debug.Log("Game Over!");
                }
            }
            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.UpdateLivesCounter(lives);
            }
        }
    }


    // Method to set the safe space state
    public void SetSafeSpace(bool isSafe)
    {
        isInSafeSpace = isSafe;
    }

    // Utility method for other scripts to check current lives.
    public int GetCurrentLives()
    {
        return lives;
    }
}
