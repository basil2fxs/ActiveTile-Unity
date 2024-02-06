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
        tileRenderer.material = isActive ? activeMaterial : inactiveMaterial;
    }

    // Method to simulate stepping on the tile via mouse click
    void OnMouseDown()
    {
        // Toggle the tile's active state on mouse click
        bool isActive = tileRenderer.material == inactiveMaterial;
        SetActiveState(isActive);

        // Log the tile's name when clicked
        Debug.Log($"{gameObject.name} has been activated by clicking.");
    }
}
