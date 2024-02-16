using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    private string selectedConfigName = "";
    private TileScript.TileState selectedState = TileScript.TileState.Neutral;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Draw the default inspector

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Bulk Add Tiles to Configuration", EditorStyles.boldLabel);

        // Configuration name selection or input
        selectedConfigName = EditorGUILayout.TextField("Config Name:", selectedConfigName);

        // State selection for new tiles
        selectedState = (TileScript.TileState)EditorGUILayout.EnumPopup("Tile State:", selectedState);

        if (GUILayout.Button("Add Selected Tiles to Config"))
        {
            AddSelectedTilesToConfig();
        }
        DrawDefaultInspector();
    }

    private void AddSelectedTilesToConfig()
    {
        var manager = (LevelManager)target;
        var selectedTiles = new List<GameObject>();

        // Assuming you've selected GameObjects in the editor
        foreach (GameObject obj in Selection.gameObjects)
        {
            if (obj.GetComponent<TileScript>() != null)
            {
                selectedTiles.Add(obj);
            }
        }

        if (selectedTiles.Count > 0)
        {
            // Update this method name to match the new one in LevelManager
            manager.AddOrUpdateTileInConfiguration(selectedConfigName, selectedTiles, selectedState);
            // This line marks the LevelManager object as requiring a save, so changes persist.
            EditorUtility.SetDirty(manager);
        }
        else
        {
            Debug.LogWarning("No tiles selected or selected objects are not tiles.");
        }
    }



}
