using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // 
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
