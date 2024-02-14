using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int pointValueForBlue = 10;
    public int pointValueForRed = -5;

    // Initialize level with point values
    public void InitializeLevel(int levelNumber)
    {
        // Based on levelNumber, you can adjust point values
        switch(levelNumber)
        {
            case 1:
                pointValueForBlue = 10;
                pointValueForRed = -5;
                break;
            // Define other cases for different levels
        }

        // Initialize tiles here...
    }

    // Add other LevelManager methods here...
}
