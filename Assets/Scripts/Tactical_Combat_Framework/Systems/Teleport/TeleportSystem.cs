using System.Collections.Generic;
using UnityEngine;

namespace Tactics2D
{
    /// <summary>
    /// Handles teleport tiles and their group connections.
    /// Keeps group data separate from the GridManager.
    /// </summary>
    public class TeleportSystem : MonoBehaviour
    {
        public static TeleportSystem Instance { get; private set; }

        private readonly Dictionary<Vector3Int, string> teleportGroups = new();     // tile -> group
        private readonly Dictionary<string, List<Vector3Int>> groupToTiles = new(); // group -> positions
        private readonly Dictionary<Vector3Int, GameObject> teleportEffects = new(); // tile -> effect

        private GridManager grid;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public void Initialize(GridManager gridManager)
        {
            grid = gridManager;
            teleportGroups.Clear();
            groupToTiles.Clear();
            teleportEffects.Clear();
        }

        /// <summary>
        /// Registers a teleport tile into a group.
        /// </summary>
        public void RegisterTeleport(Vector3Int pos, string group, GameObject fx = null)
        {
            teleportGroups[pos] = group;

            if (!groupToTiles.ContainsKey(group))
                groupToTiles[group] = new List<Vector3Int>();

            groupToTiles[group].Add(pos);

            if (fx != null)
                teleportEffects[pos] = fx;

            Debug.Log($"[TeleportSystem] Registered {pos} in group '{group}'");
        }

        public bool TryTeleport(Unit unit, GridCell fromCell)
        {
            if (unit == null || fromCell == null || grid == null)
                return false;

            if (!teleportGroups.TryGetValue(fromCell.GridPos, out string group))
                return false;

            if (!groupToTiles.TryGetValue(group, out var tiles) || tiles.Count < 2)
                return false;

            Vector3Int destination;
            do
            {
                destination = tiles[Random.Range(0, tiles.Count)];
            } while (destination == fromCell.GridPos && tiles.Count > 1);

            if (!grid.Cells.TryGetValue(destination, out var destTeleportCell))
                return false;

            GridCell targetNeighbor = null;
            var freeNeighbors = new List<GridCell>();

            foreach (var neighbor in grid.Neighbors(destTeleportCell))
            {
                if (neighbor.Walkable && !neighbor.IsTaken)
                    freeNeighbors.Add(neighbor);
            }

            if (freeNeighbors.Count > 0)
                targetNeighbor = freeNeighbors[Random.Range(0, freeNeighbors.Count)];
            else
                targetNeighbor = destTeleportCell;

            // --- VISUAL FX: Exit ---
            if (teleportEffects.TryGetValue(fromCell.GridPos, out var fxPrefab))
                Object.Instantiate(fxPrefab, unit.transform.position, Quaternion.identity);

            Debug.Log($"[TeleportSystem] {unit.UnitName} teleports from {fromCell.GridPos} → {targetNeighbor.GridPos}");

            // --- HIDE UNIT SPRITE ---
            var renderers = unit.GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renderers)
                r.enabled = false; // hide visuals

            // (Optional) disable collider if you want to avoid interaction during teleport
            var collider = unit.GetComponent<Collider2D>();
            if (collider) collider.enabled = false;

            // --- UPDATE CELL OCCUPANCY ---
            fromCell.RemoveOccupant(unit);
            targetNeighbor.AddOccupant(unit);

            // --- PHYSICAL MOVE ---
            unit.transform.position = targetNeighbor.WorldCenter;
            unit.SetCurrentCell(targetNeighbor);

            // --- VISUAL FX: Entry ---
            if (teleportEffects.TryGetValue(destination, out var fxPrefab2))
                Object.Instantiate(fxPrefab2, targetNeighbor.WorldCenter, Quaternion.identity);

            // --- REAPPEAR AFTER DELAY ---
            unit.StartCoroutine(ReappearAfterDelay(unit, 0.2f)); // fade back in after 0.2 seconds

            // --- Notify GridManager behaviors ---
            grid.RefreshUnitPosition(unit);

            return true;
        }

    private System.Collections.IEnumerator ReappearAfterDelay(Unit unit, float delay)
    {
        yield return new WaitForSeconds(delay);

        var renderers = unit.GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers)
            r.enabled = true;

        var collider = unit.GetComponent<Collider2D>();
        if (collider) collider.enabled = true;
    }

        // ---------------- Group Queries ----------------

        public bool TryGetGroup(GridCell cell, out string group)
        {
            return teleportGroups.TryGetValue(cell.GridPos, out group);
        }

        public IEnumerable<GridCell> GetGroupCells(string group)
        {
            if (!groupToTiles.TryGetValue(group, out var tiles))
                yield break;

            foreach (var pos in tiles)
                if (grid.Cells.TryGetValue(pos, out var cell))
                    yield return cell;
        }
    }
}
