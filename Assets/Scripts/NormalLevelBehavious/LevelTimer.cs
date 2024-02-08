using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelTimer : MonoBehaviour
{
    public int timeInSeconds = 120; // Example starting time
    private Text timerText;

    void Start()
    {
        timerText = GetComponent<Text>();
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while(timeInSeconds > 0)
        {
            timeInSeconds--;
            timerText.text = $"Time: {timeInSeconds}";
            yield return new WaitForSeconds(1);
        }
        // Handle time up (e.g., end level, reduce life, etc.)
    }
}
