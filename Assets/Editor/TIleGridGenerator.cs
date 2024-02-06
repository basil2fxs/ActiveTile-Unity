using UnityEngine;
using UnityEditor;

public class TileGridGenerator : EditorWindow
{
    private GameObject tilePrefab;
    private int gridWidth = 10;
    private int gridHeight = 10;
    private float spacing = 1.1f; // Space between tiles

    [MenuItem("Tools/Generate Tile Grid")]
    public static void ShowWindow()
    {
        GetWindow<TileGridGenerator>("Tile Grid Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Grid Settings", EditorStyles.boldLabel);

        tilePrefab = (GameObject)EditorGUILayout.ObjectField("Tile Prefab", tilePrefab, typeof(GameObject), false);
        gridWidth = EditorGUILayout.IntField("Grid Width", gridWidth);
        gridHeight = EditorGUILayout.IntField("Grid Height", gridHeight);
        spacing = EditorGUILayout.FloatField("Spacing", spacing);

        if (GUILayout.Button("Generate Grid"))
        {
            GenerateGrid();
        }
    }

    void GenerateGrid()
    {
        if (tilePrefab == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign a tile prefab.", "OK");
            return;
        }

        GameObject parentObject = new GameObject("TileGrid");

        int tileNumber = 1; // Start numbering tiles from 1

        for (int y = 0; y < gridHeight; y++) // Iterate over what are now effectively horizontal rows
        {
            bool isEvenRow = y % 2 == 0;

            for (int x = 0; x < gridWidth; x++) // Iterate over what are now effectively vertical columns
            {
                int actualX = isEvenRow ? x : gridWidth - 1 - x; // Reverse direction for odd rows

                // For a clockwise rotation, adjust the calculation of position
                // Note: This assumes the origin (0,0) is at the bottom left
                Vector3 position = new Vector3((gridHeight - 1 - y) * spacing, actualX * spacing, 0);
                GameObject newTile = (GameObject)PrefabUtility.InstantiatePrefab(tilePrefab, parentObject.transform);
                newTile.transform.position = position;

                // Numbering the tile
                newTile.name = "Tile " + tileNumber++;

                // Optionally, if your Tile script or another component on the prefab needs to reference this number,
                // you could add a line here to set it, for example:
                // newTile.GetComponent<Tile>().SetTileNumber(tileNumber - 1);
            }
        }

        Selection.activeGameObject = parentObject;
    }
}