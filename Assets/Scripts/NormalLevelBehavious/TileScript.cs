using UnityEngine;
using System.Collections;


// This ensures the script can run in edit mode for immediate feedback
[ExecuteInEditMode]
public class TileScript : MonoBehaviour
{
    public enum TileState { Safe, Point, Neutral, Danger }
    public TileState currentState = TileState.Neutral;
    public Material safeMaterial, pointMaterial, neutralMaterial, dangerMaterial, triggerMaterial;
    public bool wasLastBlue = false;
    private Renderer tileRenderer; // Reference to the Renderer component

    private void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
        UpdateMaterial(); // Ensure the correct material is applied upon awakening
    }
    // This method is now public to allow external scripts (like a level editor tool) to set the tile's state
    public void SetState(TileState state, bool updateMaterialImmediately = true)
    {
        currentState = state;
        if (updateMaterialImmediately)
        {
            UpdateMaterial(); // Update the tile's appearance based on the new state
        }
    }

    void OnValidate()
    {
        // Ensures the tileRenderer is always assigned, even in the editor
        if (tileRenderer == null) tileRenderer = GetComponent<Renderer>();

        UpdateMaterial(); // Updates the material to reflect changes made in the editor
    }

    void OnMouseDown()
    {
        // This ensures that changes only happen during play mode

        //might need this:
        //if (currentState == TileState.Point)
        if (Application.isPlaying)
        {
            HandleTileInteraction();
        }
    }

    IEnumerator FlashDangerTile()
    {
        Material originalMaterial = tileRenderer.material; // Store the original material
        GameManager.instance?.DeductLife(); // Simplified null-check with '?'
        for(int i = 0; i < 6; i++)
        {
            tileRenderer.material = triggerMaterial; // Directly set to trigger material for flashing effect
            yield return new WaitForSeconds(0.5f); // Wait for 1 second
            tileRenderer.material = triggerMaterial;
        }
        tileRenderer.material = originalMaterial; // Revert to the original material
        UpdateMaterial();
    }

    private void HandleTileInteraction()
    {
        if (!(GameManager.instance.IsInSafeSpace))
        {
            switch (currentState)
            {
                case TileState.Point:
                    SetState(TileState.Neutral); // Change to neutral, simulate scoring points
                    ScoreManager.instance?.AddPoints(true); // Simplified null-check with '?'
                    LevelManager.instance?.BlueTileClaimed(); // Notify LevelManager
                    break;
                case TileState.Danger:
                    StartCoroutine(FlashDangerTile()); // Flash to indicate a life lost
                    break;
            }
        }
    }

    // A helper method to update the tile's material based on its current state
    private void UpdateMaterial()
    {
        if (tileRenderer == null) return; // Ensure this method only runs when the tileRenderer is available

        switch (currentState)
        {
            case TileState.Safe:
                tileRenderer.material = safeMaterial;
                break;
            case TileState.Point:
                tileRenderer.material = pointMaterial;
                break;
            case TileState.Neutral:
                tileRenderer.material = neutralMaterial;
                break;
            case TileState.Danger:
                tileRenderer.material = dangerMaterial;
                break;
        }
    }
}
