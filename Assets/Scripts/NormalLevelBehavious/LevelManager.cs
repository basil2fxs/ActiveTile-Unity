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
    public int blueTilesCount = 50; // This will keep track of the blue tiles
    public static LevelManager instance;
    public List<GameObject> tiles; // Ensure this list is populated with all your tile GameObjects
    public TileAnimationController tileAnimationController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        StartCoroutine(Countdown(0));
    }
    public void BlueTileClaimed()
    {
        //Debug.Log("BlueTileClaimed");
        blueTilesCount--;
        if (blueTilesCount <= 31)
        {
            StartCoroutine(AnimateTilesSafeMode());
        }
    }

    void TransitionToLevel2()
    {
        InitializeLevel(1);
    }

    IEnumerator AnimateTilesSafeMode()
    {
        // Find all tiles and turn them black
        GameManager.instance.SetSafeSpace(true);
        TileScript[] tiles = FindObjectsOfType<TileScript>();
        foreach (TileScript tile in tiles)
        {
            tile.SetState(TileScript.TileState.Neutral);
        }
        yield return new WaitForSeconds(0.1f); // Wait a bit before starting the green animation

        // Now turn them green one by one
        tileAnimationController.StartRowAnimation(tileAnimationController.greenMaterial, 0.07f, 0.07f);
        yield return new WaitForSeconds(4f); 

        // Once all tiles are green, set the game to safe mode and prepare for the next level
        GameManager.instance.SetSafeSpace(false);
        TransitionToLevel2(); //loop Level 1.1
    }
    public IEnumerator AnimateTilesFailMode()
    {
        // Find all tiles and turn them black
        GameManager.instance.SetSafeSpace(true);
        TileScript[] tiles = FindObjectsOfType<TileScript>();
        foreach (TileScript tile in tiles)
        {
            tile.SetState(TileScript.TileState.Neutral);
        }
        yield return new WaitForSeconds(0.1f); // Wait a bit before starting the green animation

        // Now turn them green one by one
        tileAnimationController.StartRowAnimation(tileAnimationController.redMaterial, 0.07f, 0.07f);
        yield return new WaitForSeconds(4f);

        // Once all tiles are green, set the game to safe mode and prepare for the next level
        //GameManager.instance.SetSafeSpace(false);
        GameManager.instance.ResetLives();
        StartCoroutine(Countdown(0));
    }

    IEnumerator Countdown(int levelIndex)
    {
        // Assuming numberSetups are correctly configured for 5 to 1 countdown
        GameManager.instance.SetSafeSpace(true);
        for(int i=0; i<5; i++)
        {
            ApplyNumberConfiguration(numberSetups[i], false);
            yield return new WaitForSeconds(1); // Wait for a second between each number
        }
        ResetTilesToNeutral();
        GameManager.instance.SetSafeSpace(false);
        InitializeLevel(levelIndex); // Proceed to initialize the first level after the countdown
    }
    public void InitializeLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelSetups.Count)
        {
            Debug.LogError("Level index out of range.");
            return;
        }
        StartCoroutine(ApplyConfiguration(levelSetups[levelIndex], false));
        TileScript[] tiles = FindObjectsOfType<TileScript>();
        blueTilesCount = tiles.Count(tile => tile.currentState == TileScript.TileState.Point);
    }
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

    IEnumerator ApplyConfiguration(TileConfiguration config, bool isNumber)
    {
        foreach (TileStateConfig tileStateConfig in config.tileConfigs)
        {
            if (tileStateConfig.tile != null)
            {
                TileScript tileScript = tileStateConfig.tile.GetComponent<TileScript>();
                if (tileScript != null)
                {
                    // Check if we're setting a safe state or if it's a number configuration
                    if (tileStateConfig.initialState == TileScript.TileState.Safe || isNumber)
                    {
                        // Only apply the state if it's safe or part of the number setup
                        tileScript.SetState(tileStateConfig.initialState);
                    }
                }
            }
        }

        // Wait a bit before starting points and red Tiles
        yield return new WaitForSeconds(2.5f);
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
    void ApplyNumberConfiguration(TileConfiguration config, bool isNumber)
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
