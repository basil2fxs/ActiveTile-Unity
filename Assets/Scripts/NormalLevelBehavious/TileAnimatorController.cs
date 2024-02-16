using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimationController : MonoBehaviour
{
    public List<AnimationSequence> rowAnimations = new List<AnimationSequence>();
    public List<AnimationSequence> columnAnimations = new List<AnimationSequence>();

    public Material greenMaterial, redMaterial, neutralMaterial;
    public float animationDelay = 0.5f; // Delay between animating each row/column
    public float revertDelay = 2.0f; // Delay before reverting to neutralMaterial

    [System.Serializable]
    public class AnimationSequence
    {
        public string name;
        public List<GameObject> tiles = new List<GameObject>();
    }

    // Initiates the animation for all row sequences
    public void StartRowAnimation(Material material, float delayBetweenTiles, float delayBeforeReverting)
    {
        StartCoroutine(AnimateTiles(rowAnimations, material, delayBetweenTiles, delayBeforeReverting));
    }

    public void StartColumnAnimation(Material material, float delayBetweenTiles, float delayBeforeReverting)
    {
        StartCoroutine(AnimateTiles(columnAnimations, material, delayBetweenTiles, delayBeforeReverting));
    }


    public void AnimateSequence(List<AnimationSequence> sequences, Material startMaterial, float startDelay, float revertStartDelay)
    {
        StartCoroutine(AnimateTiles(sequences, startMaterial, startDelay, revertStartDelay));
    }

    IEnumerator AnimateTiles(List<AnimationSequence> sequences, Material startMaterial, float startDelay, float revertStartDelay)
    {
        foreach (var sequence in sequences)
        {
            foreach (var tile in sequence.tiles)
            {
                if (tile != null)
                {
                    Renderer tileRenderer = tile.GetComponent<Renderer>();
                    if (tileRenderer != null)
                    {
                        tileRenderer.material = startMaterial;
                    }
                }
            }
            yield return new WaitForSeconds(startDelay); // Delay for each tile's animation
        }
         // Optionally revert each tile to neutralMaterial after specified delay
         foreach (var sequence in sequences)
        {
            foreach (var tile in sequence.tiles)
            {
                if (tile != null)
                {
                    Renderer tileRenderer = tile.GetComponent<Renderer>();
                    if (tileRenderer != null)
                    {
                        tileRenderer.material = neutralMaterial;
                    }
                }
            }
            yield return new WaitForSeconds(startDelay); // Delay for each tile's animation
        }
    }
}
