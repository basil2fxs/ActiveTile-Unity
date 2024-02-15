using UnityEngine;
using UnityEngine.UI; // Use UnityEngine.UI.TMP if you're using TextMeshPro

public class UIManager : MonoBehaviour
{
    public Text livesCounterText; // Reference to the Text component for lives counter

    private void OnEnable()
    {
        //UpdateLivesCounter(GameManager.instance.GetCurrentLives());
        // Optionally, subscribe to a custom event in GameManager that's called when lives change
    }

    private void Start()
    {
        // Ensure the UI matches the current game state at start
        UpdateLivesCounter(GameManager.instance.GetCurrentLives());
        GameManager.instance.ResetLives();
    }

    public void UpdateLivesCounter(int lives)
    {
        if (livesCounterText != null)
        {
            livesCounterText.text = "Lives: " + lives;
        }
    }
}
