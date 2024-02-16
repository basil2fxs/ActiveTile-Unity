using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(TileAnimationController))]
public class TileAnimationControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the default inspector

        TileAnimationController controller = (TileAnimationController)target;

        if (GUILayout.Button("Add Selected Tiles to New Row Sequence"))
        {
            AddSelectedTilesToSequence(controller, true); // true for row sequence
        }

        if (GUILayout.Button("Add Selected Tiles to New Column Sequence"))
        {
            AddSelectedTilesToSequence(controller, false); // false for column sequence
        }

        // Include button for starting the animation if necessary
    }

    void AddSelectedTilesToSequence(TileAnimationController controller, bool isRow)
    {
        TileAnimationController.AnimationSequence newSequence = new TileAnimationController.AnimationSequence
        {
            // Name the sequence based on whether it's a row or column and the current count
            name = isRow ? "row" + controller.rowAnimations.Count : "column" + controller.columnAnimations.Count,
            tiles = new List<GameObject>()
        };

        foreach (GameObject obj in Selection.gameObjects)
        {
            if (obj.GetComponent<Renderer>() != null) // Ensure it has a Renderer
            {
                newSequence.tiles.Add(obj);
            }
        }

        if (isRow)
        {
            controller.rowAnimations.Add(newSequence);
        }
        else
        {
            controller.columnAnimations.Add(newSequence);
        }

        EditorUtility.SetDirty(controller);
    }
}
