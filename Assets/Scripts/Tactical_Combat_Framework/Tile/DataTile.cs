using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tactics2D/Terrain Tile")]
public class DataTile : Tile
{
    [Header("Terrain Settings")]
    public bool walkable = true;
    public int moveCost = 1;
    public bool blocksVision = false;
}
