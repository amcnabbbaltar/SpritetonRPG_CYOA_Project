using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tactics2D
{
    /// <summary>
    /// Central controller that builds the logical grid from a Tilemap.
    /// Handles terrain costs, highlighting, tile behaviors, and cell lookups.
    /// Teleport logic and other systems are handled by separate managers.
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        [Header("Tilemap References")]
        [SerializeField] private Tilemap dataTilemap;
        [SerializeField] private Tilemap highlightTilemap;
        [SerializeField] private TileBase highlightTile;

        [Header("External Systems")]
        [SerializeField] private TeleportSystem teleportSystem; // external modular system
        [SerializeField] private PressurePlateSystem pressurePlateSystem; // external modular system
        [SerializeField] private ActionSystem actionSystem; // external modular system

        private readonly Dictionary<Vector3Int, GridCell> cells = new();
        private readonly Dictionary<Vector3Int, int> moveCost = new();
        private readonly Dictionary<Vector3Int, ITileBehavior> tileBehaviors = new();

        public static GridManager Instance { get; private set; }
        public IReadOnlyDictionary<Vector3Int, GridCell> Cells => cells;

        private void Awake()
        {
            Instance = this;

            // Initialize linked systems
            if (teleportSystem != null)
                teleportSystem.Initialize(this);
            if (pressurePlateSystem != null)
                pressurePlateSystem.Initialize(this);
            if (actionSystem != null)
                actionSystem.Initialize(this );
            BuildGridFromTilemap();
        }

        /// <summary>
        /// Scans the Tilemap and builds a logical grid of cells with movement and terrain data.
        /// </summary>
        public void BuildGridFromTilemap()
        {
            cells.Clear();
            moveCost.Clear();
            tileBehaviors.Clear();

            var bounds = dataTilemap.cellBounds;

            // Pass 1: build base cells and terrain
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int p = new(x, y, 0);
                if (!dataTilemap.HasTile(p)) continue;

                Vector3 worldCenter = dataTilemap.GetCellCenterWorld(p);
                var cell = new GridCell(p, worldCenter);
                cells[p] = cell;

                // Default values
                cell.Walkable = true;
                cell.BlocksVision = false;
                moveCost[p] = 1;

                if (dataTilemap.GetTile(p) is DataTile dataTile)
                {
                    cell.Walkable = dataTile.walkable;
                    cell.BlocksVision = dataTile.blocksVision;
                    moveCost[p] = Mathf.Max(1, dataTile.moveCost);
                }
            }

            // Pass 2: detect behavior tiles (Teleport, Switches, etc.)
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int p = new(x, y, 0);
                if (!dataTilemap.HasTile(p)) continue;

                if (dataTilemap.GetTile(p) is BehaviorTile bt && bt.behaviorAsset is ITileBehavior behavior)
                {
                    var instance = (Object) Object.Instantiate(bt.behaviorAsset) as ITileBehavior;
                    tileBehaviors[p] = instance;
                    instance.Initialize(this, p);
                }
            }

            Debug.Log($"[GridManager] Built {cells.Count} cells, {tileBehaviors.Count} behaviors.");
        }
        public void RefreshUnitPosition(Unit unit)
        {
            if (unit == null || unit.CurrentCell == null) return;
            OnUnitEnterCell(unit, unit.CurrentCell);
        }
        // ---------------- CELL ACCESS ----------------

        public GridCell CellFromWorld(Vector3 worldPos)
        {
            Vector3Int gp = dataTilemap.WorldToCell(worldPos);
            cells.TryGetValue(gp, out var cell);
            return cell;
        }

        public IEnumerable<GridCell> Neighbors(GridCell cell)
        {
            var dirs = new[]
            {
                new Vector3Int(1, 0, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(0, -1, 0)
            };

            foreach (var d in dirs)
            {
                var np = cell.GridPos + d;
                if (cells.TryGetValue(np, out var n) && n.Walkable)
                    yield return n;
            }
        }

        public int GetCost(GridCell from, GridCell to)
        {
            return moveCost.TryGetValue(to.GridPos, out int c) ? c : 1;
        }

        // ---------------- BEHAVIOR EVENTS ----------------

        public void OnUnitEnterCell(Unit unit, GridCell cell)
        {
            if (tileBehaviors.TryGetValue(cell.GridPos, out var behavior))
                behavior.OnUnitEnter(unit, this);
        }

        public void OnUnitExitCell(Unit unit, GridCell cell)
        {
            if (tileBehaviors.TryGetValue(cell.GridPos, out var behavior))
                behavior.OnUnitExit(unit, this);
        }

        // ---------------- ACTION SYSTEM ----------------

        public void ToggleWalkable(Vector3Int pos)
        {
            if (cells.TryGetValue(pos, out var cell))
            {
                
                cells[pos].Walkable = !cells[pos].Walkable;
                cells[pos].BlocksVision = !cells[pos].BlocksVision;
            }
        }

        // ---------------- HIGHLIGHTING ----------------

        public void ClearHighlight() => highlightTilemap.ClearAllTiles();

        public void Highlight(IEnumerable<GridCell> list)
        {
            ClearHighlight();
            foreach (var c in list)
                highlightTilemap.SetTile(c.GridPos, highlightTile);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (cells == null) return;
            foreach (var c in cells.Values)
            {
                Gizmos.color = c.Walkable ? Color.green : Color.red;
                Gizmos.DrawWireCube(c.WorldCenter, Vector3.one * 0.9f);
            }
        }
#endif
    }
}
