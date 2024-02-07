using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material activeMaterial; // Assign in the inspector
    public Material inactiveMaterial; // Assign in the inspector
    private Renderer tileRenderer;

    void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
    }

    public void SetActiveState(bool isActive)
    {
        // Directly set the material based on the isActive flag
        tileRenderer.material = isActive ? activeMaterial : inactiveMaterial;
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
