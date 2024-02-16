using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button resetScoreButton; // Ensure this is linked in the inspector

    void Start()
    {
        if (resetScoreButton != null)
        {
            resetScoreButton.onClick.RemoveAllListeners();
            resetScoreButton.onClick.AddListener(ScoreManager.instance.ResetScore);
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void PlayBenchMark()
    {
        SceneManager.LoadScene("Benchmark");
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
