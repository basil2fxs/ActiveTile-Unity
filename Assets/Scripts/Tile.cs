using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material activeMaterial;
    public Material inactiveMaterial;
    private Renderer tileRenderer;
    private bool isActive = false; // Added to track the tile's state

    void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
        if (tileRenderer == null)
        {
            Debug.LogError("Renderer component not found.", this);
        }
    }

    public void SetActiveState(bool isActive)
    {
        this.isActive = isActive; // Update the state
        if (tileRenderer != null)
        {
            tileRenderer.material = isActive ? activeMaterial : inactiveMaterial;
        }
        else
        {
            Debug.LogWarning("Trying to access a destroyed Renderer.");
        }
    }

    // Added to get the tile's state
    public bool IsActive()
    {
        return isActive;
    }

    void OnMouseDown()
    {
        // Activate the tile when the mouse button is pressed
        SetActiveState(true);
        Debug.Log($"{gameObject.name} has been activated by clicking.");
    }

    void OnMouseUp()
    {
        // Deactivate the tile when the mouse button is released
        SetActiveState(false);
    }

    void OnMouseExit()
    {
        // Optional: Deactivate the tile if the mouse exits while pressed
        SetActiveState(false);
    }
}
