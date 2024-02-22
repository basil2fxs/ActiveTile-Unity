using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


// This ensures the script can run in edit mode for immediate feedback
[ExecuteInEditMode]
public class TileScript : MonoBehaviour
{
    public enum TileState { Safe, Point, Neutral, Danger }
    public TileState currentState = TileState.Neutral;
    public Material safeMaterial, pointMaterial, neutralMaterial, dangerMaterial, triggerMaterial;
    public bool wasLastBlue = false;
    private Renderer tileRenderer; // Reference to the Renderer component
     private float lastFlashTime = 0f;

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
            yield return new WaitForSeconds(0.4f);
            tileRenderer.material = dangerMaterial;
            yield return new WaitForSeconds(0.2f);
        }
        tileRenderer.material = originalMaterial; // Revert to the original material
        UpdateMaterial();
    }

    public void HandleTileInteraction()
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
                    // Check if more than 1 second has passed since the last flash
                    if (Time.time - lastFlashTime >= 2f)
                    {
                        StartCoroutine(FlashDangerTile()); // Flash to indicate a life lost
                        lastFlashTime = Time.time; // Update the last flash time
                    }
                    break;
            }
        }
    }

    // A helper method to update the tile's material based on its current state
    public void UpdateMaterial()
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
    public string GetCurrentMaterialState()
    {
        // Use the material's name for comparison
        string currentMaterialName = tileRenderer.material.name;

        // Remove the " (Instance)" part that Unity adds to instantiated material names at runtime
        currentMaterialName = currentMaterialName.Replace(" (Instance)", "");

        if (currentMaterialName == safeMaterial.name)
            return "green";
        else if (currentMaterialName == pointMaterial.name)
            return "blue";
        else if (currentMaterialName == neutralMaterial.name)
            return "black";
        else if (currentMaterialName == dangerMaterial.name)
            return "red";
        else if (currentMaterialName == triggerMaterial.name)
            return "yellow";
        else
            return "unknown";
    }


}
