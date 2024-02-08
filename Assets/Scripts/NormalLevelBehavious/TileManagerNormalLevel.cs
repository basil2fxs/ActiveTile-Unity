using System.Collections.Generic;
using UnityEngine;

public class TileManagerNormalLevel : MonoBehaviour
{
    private Dictionary<int, TileNormalLevel> tileDictionary = new Dictionary<int, TileNormalLevel>();

    void Awake()
    {
        // Find all TileNormalLevel objects instead of Tile
        TileNormalLevel[] tiles = FindObjectsOfType<TileNormalLevel>();
        foreach (TileNormalLevel tile in tiles)
        {
            int tileNumber = int.Parse(tile.gameObject.name.Split(' ')[1]);
            tileDictionary.Add(tileNumber, tile);
        }
    }

    public void SetTileState(int tileIndex, bool isActive)
    {
        // Update the method to work with TileNormalLevel
        if (tileDictionary.TryGetValue(tileIndex, out TileNormalLevel tile))
        {
            tile.SetActiveState(isActive);
        }
    }

    public void UpdateTilesInRange(string hexData, int startTile, int endTile)
    {
        int direction = startTile <= endTile ? 1 : -1;
        int tileCount = Mathf.Abs(endTile - startTile) + 1;
        for (int i = 0; i < tileCount; i++)
        {
            int tileIndex = startTile + (i * direction);
            string tileStateHex = hexData.Substring(i * 2, 2);
            bool isActive = tileStateHex == "0A";
            SetTileState(tileIndex, isActive);
        }
    }

    // Adjusted to use the new IsActive method in TileNormalLevel class
    public bool GetTileState(int tileIndex)
    {
        // Use TileNormalLevel in the TryGetValue call
        if (tileDictionary.TryGetValue(tileIndex, out TileNormalLevel tile))
        {
            return tile.IsActive(); // Use the IsActive method to get the tile's state
        }
        return false;
    }
}
