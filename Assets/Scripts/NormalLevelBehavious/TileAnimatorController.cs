using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimationController : MonoBehaviour
{
    public List<AnimationSequence> rowAnimations = new List<AnimationSequence>();
    public List<AnimationSequence> columnAnimations = new List<AnimationSequence>();
    public float animationDelay = 0.5f; // Delay between animating each row/column
    public float revertDelay = 2.0f; // Delay before reverting to neutralMaterial

    [System.Serializable]
    public class AnimationSequence
    {
        public string name;
        public List<GameObject> tiles = new List<GameObject>();
    }

    // Initiates the animation for all row sequences
     // Initiates the animation for all row sequences
    public void StartRowAnimation(TileScript.TileState state, float delay, bool inGameCond)
    {
        StartCoroutine(AnimateTiles(rowAnimations, state, delay, inGameCond));
    }

    public void StartColumnAnimation(TileScript.TileState state, float delay, bool inGameCond)
    {
        StartCoroutine(AnimateTiles(columnAnimations, state, delay, inGameCond));
    }

    public void AnimateSequence(List<AnimationSequence> sequences, TileScript.TileState state, float delay, bool inGameCond)
    {
        StartCoroutine(AnimateTiles(sequences, state, delay, inGameCond));
    }

    IEnumerator AnimateTiles(List<AnimationSequence> sequences, TileScript.TileState state, float delay, bool inGameCond)
    {
        if (inGameCond) // Only animate if the game condition is met
        {
            List<TileScript> previouslyAnimatedTiles = new List<TileScript>(); // To keep track of tiles animated in the last sequence

            foreach (var sequence in sequences)
            {
                if (GameManager.isGameOver)
                {
                    yield break; // Exit the coroutine early
                }
                // Reset previously animated tiles to their original state unless they were safe
                foreach (var prevTileScript in previouslyAnimatedTiles)
                {
                    if (prevTileScript.currentState == TileScript.TileState.Danger)
                    {
                        // If the tile was previously turned red by the animation, and it was originally blue,
                        // revert it back to blue instead of making it neutral
                        prevTileScript.SetState(prevTileScript.wasLastBlue ? TileScript.TileState.Point : TileScript.TileState.Neutral, true);
                    }
                    if (GameManager.isGameOver)
                    {
                        yield break; // Exit the coroutine early
                    }
                }

                previouslyAnimatedTiles.Clear(); // Clear the list for the next sequence

                // Animate the current sequence of tiles
                foreach (var tile in sequence.tiles)
                {
                    if (tile != null)
                    {
                        TileScript tileScript = tile.GetComponent<TileScript>();
                        if (tileScript != null && tileScript.currentState != TileScript.TileState.Safe)
                        {
                            // Remember the tile's original state before changing it
                            tileScript.wasLastBlue = tileScript.currentState == TileScript.TileState.Point;

                            // Now, animate the current tile
                            tileScript.SetState(state, true);

                            // Add this tile to the list of previously animated tiles
                            previouslyAnimatedTiles.Add(tileScript);
                        }
                    }
                }

                // Wait for the specified delay before continuing to the next sequence
                yield return new WaitForSeconds(delay);
            }

            // Optionally, reset the last sequence of tiles back to their original state
            foreach (var tileScript in previouslyAnimatedTiles)
            {
                if (tileScript.currentState == TileScript.TileState.Danger)
                {
                    tileScript.SetState(tileScript.wasLastBlue ? TileScript.TileState.Point : TileScript.TileState.Neutral, true);
                }
            }
        }
        if (!inGameCond)//animations pre and post game
        {
            foreach (var sequence in sequences)
            {
                foreach (var tile in sequence.tiles)
                {
                    if (tile != null)
                    {
                        TileScript tileScript = tile.GetComponent<TileScript>();
                        if (tileScript != null)
                        {
                            if(state == TileScript.TileState.Danger)
                            {
                                tileScript.SetState(TileScript.TileState.Danger, true);
                            }
                            else if(state == TileScript.TileState.Safe)
                            {
                                tileScript.SetState(TileScript.TileState.Safe, true);
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(delay);
            }
            foreach (var sequence in sequences)
            {
                foreach (var tile in sequence.tiles)
                {
                    if (tile != null)
                    {
                        TileScript tileScript = tile.GetComponent<TileScript>();
                        if (tileScript != null)
                        {
                            tileScript.SetState(TileScript.TileState.Neutral, true);
                        }
                    }
                }
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
