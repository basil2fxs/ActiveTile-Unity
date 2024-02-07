using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material activeMaterial; // Assign in the inspector
    public Material inactiveMaterial; // Assign in the inspector
    private Renderer tileRenderer;

    void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
        if (tileRenderer == null)
        {
            Debug.LogError("MeshRenderer component not found.", this);
        }
    }


    public void SetActiveState(bool isActive)
    {
        if (tileRenderer != null) // Check if the MeshRenderer reference is not null
        {
            tileRenderer.material = isActive ? activeMaterial : inactiveMaterial;
        }
        else
        {
            Debug.LogWarning("Trying to access a destroyed MeshRenderer.");
        }
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
