using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material activeMaterial;
    public Material inactiveMaterial;
    private Renderer tileRenderer;
    private bool isActive = false; // Track the tile's state
    private bool isMouseDown = false; // Track if the mouse button is held down

    void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
        if (tileRenderer == null)
        {
            Debug.LogError("Renderer component not found.", this);
        }
    }

    // Added to get the tile's state
    public bool IsActive()
    {
        return isActive;
    }
    void Update()
    {
        // Check if the mouse button is still being held down
        if (isMouseDown)
        {
            SetActiveState(true);
        }
    }

    void OnMouseDown()
    {
        isMouseDown = true; // Set the flag when the mouse button is pressed
        SetActiveState(true);
    }

    void OnMouseUp()
    {
        isMouseDown = false; // Clear the flag when the mouse button is released
        SetActiveState(false);
    }

    public void SetActiveState(bool isActive)
    {
        // Check if the object or its components have been destroyed
        if (this == null || gameObject == null || tileRenderer == null)
        {
            Debug.LogWarning("Trying to access a component on a destroyed object.");
            return; // Exit the method to avoid accessing destroyed components
        }

        this.isActive = isActive; // Update the state
        tileRenderer.material = isActive ? activeMaterial : inactiveMaterial;
    }

}
