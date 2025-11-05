using System.Collections.Generic;
using UnityEngine;

namespace Tactics2D
{
    public static class Pathfinder
    {
        class Node
        {
            public GridCell cell;
            public Node parent;
            public int g; // cost from start
            public int h; // heuristic
            public int f => g + h;
        }

        static int Heuristic(GridCell a, GridCell b)
        {
            var da = a.GridPos;
            var db = b.GridPos;
            return Mathf.Abs(da.x - db.x) + Mathf.Abs(da.y - db.y);
        }

        /// <summary>
        /// A* pathfinding that considers terrain costs and teleport exits (neighboring cells).
        /// </summary>
        public static List<GridCell> FindPath(
            GridManager grid,
            GridCell start,
            GridCell goal,
            System.Func<GridCell, GridCell, int> moveCost = null)
        {
            if (start == null || goal == null) return null;
            if (goal.Occupant != null && goal != start) return null;

            var open = new List<Node>();
            var closed = new HashSet<GridCell>();
            var nodeMap = new Dictionary<GridCell, Node>();

            Node StartNode(GridCell c)
            {
                var n = new Node { cell = c, g = 0, h = Heuristic(c, goal) };
                nodeMap[c] = n;
                return n;
            }

            open.Add(StartNode(start));

            while (open.Count > 0)
            {
                open.Sort((a, b) => a.f.CompareTo(b.f));
                var cur = open[0];
                open.RemoveAt(0);

                if (cur.cell == goal)
                    return ReconstructPath(cur);

                closed.Add(cur.cell);

                // ---- Normal movement neighbors ----
                foreach (var nCell in grid.Neighbors(cur.cell))
                    ProcessNeighbor(grid, cur, nCell, goal, moveCost, open, closed, nodeMap);

                // ---- Teleport-based movement ----
                if (TeleportSystem.Instance.TryGetGroup(cur.cell, out string group))
                {
                    foreach (var destTeleport in TeleportSystem.Instance.GetGroupCells(group))
                    {
                        if (destTeleport == cur.cell) continue;
                        // Find free neighbors around the destination teleport
                        foreach (var exitNeighbor in grid.Neighbors(destTeleport))
                        {
                            if (!exitNeighbor.Walkable || exitNeighbor.Occupant != null)
                                continue;

                            // Treat teleport jump as instant (zero cost)
                            ProcessNeighbor(grid, cur, exitNeighbor, goal, moveCost, open, closed, nodeMap, teleportJump: true);
                        }
                    }
                }
            }

            return null;
        }

        private static List<GridCell> ReconstructPath(Node end)
        {
            var path = new List<GridCell>();
            for (var t = end; t != null; t = t.parent)
                path.Add(t.cell);
            path.Reverse();
            return path;
        }

        // Helper to process A* neighbor nodes
        private static void ProcessNeighbor(
            GridManager grid,
            Node cur,
            GridCell nCell,
            GridCell goal,
            System.Func<GridCell, GridCell, int> moveCost,
            List<Node> open,
            HashSet<GridCell> closed,
            Dictionary<GridCell, Node> nodeMap,
            bool teleportJump = false)
        {
            if (nCell.Occupant != null && nCell != goal) return;
            if (closed.Contains(nCell)) return;

            int cost = teleportJump ? 0 : (moveCost?.Invoke(cur.cell, nCell) ?? 1);
            int tentativeG = cur.g + cost;

            if (!nodeMap.TryGetValue(nCell, out var nNode))
            {
                nNode = new Node { cell = nCell };
                nodeMap[nCell] = nNode;
                open.Add(nNode);
            }
            else if (tentativeG >= nNode.g)
                return;

            nNode.parent = cur;
            nNode.g = tentativeG;
            nNode.h = Heuristic(nCell, goal);
        }

        /// <summary>
        /// BFS range fill (used for movement range highlighting),
        /// also includes teleport exit cells.
        /// </summary>
        public static HashSet<GridCell> FloodFill(GridManager grid, GridCell start, int maxCost, System.Func<GridCell, GridCell, int> moveCost = null)
        {
            var visited = new Dictionary<GridCell, int> { [start] = 0 };
            var q = new Queue<GridCell>();
            q.Enqueue(start);

            while (q.Count > 0)
            {
                var cur = q.Dequeue();

                // ---- Normal neighbors ----
                foreach (var n in grid.Neighbors(cur))
                {
                    int cost = (moveCost?.Invoke(cur, n) ?? 1) + visited[cur];
                    if (cost > maxCost) continue;
                    if (n.Occupant != null && n != start) continue;

                    if (!visited.TryGetValue(n, out int prev) || cost < prev)
                    {
                        visited[n] = cost;
                        q.Enqueue(n);
                    }
                }

                // ---- Teleport exits ----
                if (TeleportSystem.Instance.TryGetGroup(cur, out string group))
                {
                    foreach (var destTeleport in TeleportSystem.Instance.GetGroupCells(group))
                    {
                        if (destTeleport == cur) continue;

                        foreach (var exitNeighbor in grid.Neighbors(destTeleport))
                        {
                            if (!exitNeighbor.Walkable || exitNeighbor.Occupant != null)
                                continue;

                            int cost = visited[cur]; // teleport = 0 cost
                            if (!visited.ContainsKey(exitNeighbor) || cost < visited[exitNeighbor])
                            {
                                visited[exitNeighbor] = cost;
                                q.Enqueue(exitNeighbor);
                            }
                        }
                    }
                }
            }

            return new HashSet<GridCell>(visited.Keys);
        }
    }
}
