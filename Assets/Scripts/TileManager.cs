using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private Dictionary<int, Tile> tileDictionary = new Dictionary<int, Tile>();

    void Awake()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            int tileNumber = int.Parse(tile.gameObject.name.Split(' ')[1]);
            tileDictionary.Add(tileNumber, tile);
        }
    }

    public void SetTileState(int tileIndex, bool isActive)
    {
        if (tileDictionary.TryGetValue(tileIndex, out Tile tile))
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

    // Adjusted to use the new IsActive method in Tile class
    public bool GetTileState(int tileIndex)
    {
        if (tileDictionary.TryGetValue(tileIndex, out Tile tile))
        {
            return tile.IsActive(); // Use the IsActive method to get the tile's state
        }
        return false;
    }
}
