using System.Collections.Generic;
using UnityEngine;

public class TileManagerNormalLevel : MonoBehaviour
{
    private Dictionary<int, TileNormalLevel> tileDictionary = new Dictionary<int, TileNormalLevel>(); //change for tile script

    void Awake()
    {
        TileNormalLevel[] tiles = FindObjectsOfType<TileNormalLevel>(); //change for tile script
        foreach (TileNormalLevel tile in tiles)
        {
            int tileNumber = int.Parse(tile.gameObject.name.Split(' ')[1]);
            tileDictionary.Add(tileNumber, tile);
        }
    }

    //trigger behaviours if active
    public void SetTileState(int tileIndex, bool isActive) //gets called by UDP reciever (tile number, 1 for active 0 for inactive)
    {
        if (tileDictionary.TryGetValue(tileIndex, out TileNormalLevel tile)) //change for tile script
        {
            if(isActive == true)
            {
                NormalLevelBehaviours(tileIndex, GetTileState(tileIndex)); //get a int colour -> 1balck,2red,3orange,4yellow,5green,6blue,7cyan,8pruple,9pink
            }
        }
    }

    //behaviours for normal level
    public void NormalLevelBehaviours(int tileIndex, int currentColour)
    {
        if (tileDictionary.TryGetValue(tileIndex, out TileNormalLevel tile)) //change for tile script
        {
            if(currentColour == 6) //if blue
            {
                currentColour = 1; //make blue -> black when stepped
                tile.SetActiveState(currentColour);
            }
            else
            {
                //dont change green or black tiles
            }
            if(currentColour == 2) //if red stepped flash that tile yellow
            {
                //currentColour = FlashYellow(tileIndex);
            }
        }
    }

    /* Add Flash Yellow Later
    public int FlashYellow(int tileIndex)
    {
        if (tileDictionary.TryGetValue(tileIndex, out TileNormalLevel tile)) //change for tile script
        {
            for(int i = 0; i<5; i++)
            {
                tile.SetActiveState(4);
                yield return new WaitForSeconds(0.5f); // Flash on duration
                tile.SetActiveState(2);
                yield return new WaitForSeconds(0.25f); // Flash off duration
            }
        }
        return 2;//dont change from red
    }
    */

    public int GetTileState(int tileIndex)
    {
        if (tileDictionary.TryGetValue(tileIndex, out TileNormalLevel tile)) //change for tile script
        {
            return tile.IsActive(); // Use the IsActive method to get the tile's colour
        }
        return 1;
    }
}
