using UnityEngine;

public class TileScript : MonoBehaviour
{
    public enum TileType { Safe, Point, Neutral, Avoid }
    private TileType type;
    private LevelManager levelManager;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>(); // Ensure there's only one LevelManager in the scene
    }

    public void SetType(TileType newType)
    {
        type = newType;
    }

    void OnMouseDown()
    {
        switch (type)
        {
            case TileType.Point:
                GameManager.instance.AddScore(GameManager.instance.teamName, levelManager.pointValueForBlue);
                // Change to neutral...
                break;
            case TileType.Avoid:
                GameManager.instance.AddScore(GameManager.instance.teamName, levelManager.pointValueForRed);
                // Flash orange, then turn black or disappear...
                break;
        }
    }
}
