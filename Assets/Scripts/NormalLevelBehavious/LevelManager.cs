using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class TileStateConfig
    {
        public GameObject tile;
        public TileScript.TileState initialState;
    }

    [System.Serializable]
    public class TileConfiguration
    {
        public string name;
        public List<TileStateConfig> tileConfigs = new List<TileStateConfig>();
    }

    public List<TileConfiguration> levelSetups = new List<TileConfiguration>();
    public List<TileConfiguration> numberSetups = new List<TileConfiguration>(); // Assume these are for the countdown visuals

    void Start()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        // Assuming numberSetups are correctly configured for 5 to 1 countdown
        for(int i=0; i<5; i++)
        {
            ApplyConfiguration(numberSetups[i], false);
            yield return new WaitForSeconds(1); // Wait for a second between each number
        }
        InitializeLevel(0); // Proceed to initialize the first level after the countdown
    }
    public void InitializeLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelSetups.Count)
        {
            Debug.LogError("Level index out of range.");
            return;
        }
        ApplyConfiguration(levelSetups[levelIndex], false);
    }
    // Inside LevelManager.cs

    // Inside LevelManager.cs

    public void AddOrUpdateTileInConfiguration(string configName, List<GameObject> tiles, TileScript.TileState initialState)
    {
        // Find an existing configuration by name or create a new one if it doesn't exist
        var config = levelSetups.FirstOrDefault(c => c.name == configName);
        if (config == null)
        {
            config = new TileConfiguration { name = configName };
            levelSetups.Add(config);
        }

        foreach (var tile in tiles)
        {
            var existingTileConfig = config.tileConfigs.FirstOrDefault(tc => tc.tile == tile);
            if (existingTileConfig != null)
            {
                // If the tile already exists in the configuration, update its state
                existingTileConfig.initialState = initialState;
            }
            else
            {
                // If the tile is not already in the configuration, add it
                config.tileConfigs.Add(new TileStateConfig
                {
                    tile = tile,
                    initialState = initialState
                });
            }
        }

        // Ensure changes are saved
    }

    
    void ResetTilesToNeutral()
    {
        // Loop through all tiles and reset them to a neutral state
        // This method assumes you have a way to access all tile GameObjects, possibly stored in a list or through tagging
        foreach (var tile in FindObjectsOfType<TileScript>()) // Example using FindObjectsOfType, adjust based on your setup
        {
            tile.SetState(TileScript.TileState.Neutral); // Reset tile to neutral state
        }
    }

    void ApplyConfiguration(TileConfiguration config, bool isNumber)
    {
        foreach (TileStateConfig tileStateConfig in config.tileConfigs)
        {
            if (tileStateConfig.tile != null)
            {
                TileScript tileScript = tileStateConfig.tile.GetComponent<TileScript>();
                if (tileScript != null)
                {
                    if (isNumber)
                    {
                        // For numbers, you might want to change materials or use a specific visual representation
                        // This example doesn't implement material change logic directly, but it's set up for such customization
                        tileScript.SetState(tileStateConfig.initialState, false);
                        // Optionally apply numberMaterial or any specific handling for countdown numbers
                    }
                    else
                    {
                        tileScript.SetState(tileStateConfig.initialState);
                    }
                }
            }
        }
    }
}
