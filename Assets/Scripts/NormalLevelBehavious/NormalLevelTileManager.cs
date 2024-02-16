using System.Collections.Generic;
using UnityEngine;

public class NormalLevelTileManager : MonoBehaviour
{
    private Dictionary<int, TileScript> tileDictionary = new Dictionary<int, TileScript>();

    void Awake()
    {
        TileScript[] tiles = FindObjectsOfType<TileScript>();
        foreach (TileScript tile in tiles)
        {
            int tileNumber;
            // Ensure parsing is safe and ignores tiles with names that don't fit the expected format
            if (int.TryParse(tile.gameObject.name.Split(' ')[1], out tileNumber))
            {
                tileDictionary.Add(tileNumber, tile);
            }
        }
    }

    public void SetTileState(int tileIndex, bool isActive)
    {
        if (tileDictionary.TryGetValue(tileIndex, out TileScript tile))
        {
            if (Application.isPlaying)
            {
                tile.HandleTileInteraction();
            }

        }
    }
    public string GetTileState(int tileIndex)
    {
        if (tileDictionary.TryGetValue(tileIndex, out TileScript tile))
        {
            return tile.GetCurrentMaterialState(); // Use the new method to get the tile's material state as a string
        }
        return tile.GetCurrentMaterialState();
    }
}
