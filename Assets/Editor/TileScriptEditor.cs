using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(TileScript))]
[CanEditMultipleObjects] // This attribute allows multi-object editing
public class TileScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector options
        base.OnInspectorGUI();

        EditorGUILayout.Space(); // Add some spacing for clarity
        EditorGUILayout.LabelField("Bulk State Changer", EditorStyles.boldLabel);

        // Bulk state change buttons
        if (GUILayout.Button("Set All Selected Tiles to Safe"))
        {
            SetStateForSelectedTiles(TileScript.TileState.Safe);
        }
        if (GUILayout.Button("Set All Selected Tiles to Point"))
        {
            SetStateForSelectedTiles(TileScript.TileState.Point);
        }
        if (GUILayout.Button("Set All Selected Tiles to Neutral"))
        {
            SetStateForSelectedTiles(TileScript.TileState.Neutral);
        }
        if (GUILayout.Button("Set All Selected Tiles to Danger"))
        {
            SetStateForSelectedTiles(TileScript.TileState.Danger);
        }
    }
    
    private void SetStateForSelectedTiles(TileScript.TileState newState)
    {
        // Begin a group of undo operations
        Undo.RecordObjects(targets, "Change Tile State");

        foreach (var targetObject in targets) // Assuming 'targets' is an array of all selected objects this editor can edit
        {
            TileScript tileScript = targetObject as TileScript; // Safely cast to TileScript
            if (tileScript != null)
            {
                tileScript.SetState(newState); // Apply the newState dynamically based on the button pressed
                EditorUtility.SetDirty(tileScript); // Mark the tileScript as dirty to ensure the change is saved
            }
        }
    }
}
