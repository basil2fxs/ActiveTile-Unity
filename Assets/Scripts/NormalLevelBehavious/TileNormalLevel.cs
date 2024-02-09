using UnityEngine;


public class TileNormalLevel : MonoBehaviour
{
    public TileManagerNormalLevel tileManager; // change for the tile manager
    public Material black;
    public Material red;
    public Material orange;
    public Material yellow;
    public Material green;
    public Material blue;
    public Material cyan;
    public Material purple;
    public Material pink;
    private Renderer tileRenderer;
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
    public int IsActive()
    {
        int colourState = 1;
        if (tileRenderer.material == black) {
            colourState = 1;
        } else if (tileRenderer.material == red) {
            colourState = 2;
        } else if (tileRenderer.material == orange) {
            colourState = 3;
        } else if (tileRenderer.material == yellow) {
            colourState = 4;
        } else if (tileRenderer.material == green) {
            colourState = 5;
        } else if (tileRenderer.material == blue) {
            colourState = 6;
        } else if (tileRenderer.material == cyan) {
            colourState = 7;
        } else if (tileRenderer.material == purple) {
            colourState = 8;
        } else if (tileRenderer.material == pink) {
            colourState = 9;
        }
        return colourState;
    }


    void Update()
    {
        // Check if the mouse button is still being held down
        if (isMouseDown)
        {
            string objectName = gameObject.name;
            string[] parts = objectName.Split(' ');
            string numberPart = parts[parts.Length - 1]; // Split the name by space character and get the last element
            if (int.TryParse(numberPart, out int number))
            {
                // 'number' is now an integer that you can use\
                tileManager.SetTileState(number, true);
            }
        }
    }

    void OnMouseDown()
    {
        isMouseDown = true; // Set the flag when the mouse button is pressed
        string objectName = gameObject.name;
        string[] parts = objectName.Split(' ');
        string numberPart = parts[parts.Length - 1]; // Split the name by space character and get the last element
        if (int.TryParse(numberPart, out int number))
        {
            // 'number' is now an integer that you can use\
            tileManager.SetTileState(number, true);
        }
    }

    void OnMouseUp()
    {
        isMouseDown = false; // Clear the flag when the mouse button is released
        string objectName = gameObject.name;
        string[] parts = objectName.Split(' ');
        string numberPart = parts[parts.Length - 1]; // Split the name by space character and get the last element
        if (int.TryParse(numberPart, out int number))
        {
            // 'number' is now an integer that you can use\
            tileManager.SetTileState(number, false);
        }
    }

    public void SetActiveState(int colour) //get a int colour -> 1balck,2red,3orange,4yellow,5green,6blue,7cyan,8pruple,9pink
    {
        // Check if the object or its components have been destroyed
        if (this == null || gameObject == null || tileRenderer == null)
        {
            Debug.LogWarning("Trying to access a component on a destroyed object.");
            return; // Exit the method to avoid accessing destroyed components
        }

        switch (colour)
        {
            case 1://black
                tileRenderer.material = black;
                break;
            case 2://red
                tileRenderer.material = red;
                break;
            case 3://orange
                tileRenderer.material = orange;
                break;
            case 4://yellow
                tileRenderer.material = yellow;
                break;
            case 5://green
                tileRenderer.material = green;
                break;
            case 6://blue
                tileRenderer.material = blue;
                break;
            case 7://cyan
                tileRenderer.material = cyan;
                break;
            case 8://purple
                tileRenderer.material = purple;
                break;
            case 9://pink
                tileRenderer.material = pink;
                break;
        }
    }

}
