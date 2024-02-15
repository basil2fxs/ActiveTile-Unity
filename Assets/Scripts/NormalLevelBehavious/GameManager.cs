using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int lives = 3; // Default number of lives

    void Awake()
    {
        // Ensure there's only one instance of the GameManager running.
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

    public void DeductLife()
    {
        lives--;
        // Add any UI update logic here, if needed.
        if (lives <= 0)
        {
            // Handle game over logic here
            Debug.Log("Game Over!");
        }
    }

    // Utility method for other scripts to check current lives.
    public int GetCurrentLives()
    {
        return lives;
    }
}
