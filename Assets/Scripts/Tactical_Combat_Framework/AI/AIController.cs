using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tactics2D
{
    /// <summary>
    /// Handles all AI logic: target selection, movement, and attacks.
    /// Works with the TurnManager to perform one full AI unit turn.
    /// </summary>
    public class AIController : MonoBehaviour
    {
        public IEnumerator ExecuteTurn(Unit enemy)
        {
            var grid = GridManager.Instance;
            if (enemy == null || !enemy.IsAlive || grid == null)
                yield break;

            Debug.Log($"[AIController] {enemy.UnitName} starting turn.");

            // 🔹 Find the nearest player unit
            var players = FindObjectsOfType<Unit>()
                .Where(u => u.Team == Team.Player && u.IsAlive)
                .ToList();

            if (players.Count == 0)
            {
                Debug.Log("[AIController] No player units found.");
                yield break;
            }

            Unit target = players.OrderBy(p =>
                Mathf.Abs(p.CurrentCell.GridPos.x - enemy.CurrentCell.GridPos.x) +
                Mathf.Abs(p.CurrentCell.GridPos.y - enemy.CurrentCell.GridPos.y)).First();

            int dist = GridDistance(enemy, target);

            // 🔹 If already in range → attack immediately
            if (dist <= enemy.Stats.attackRange)
            {
                Debug.Log($"[AIController] {enemy.UnitName} is in range of {target.UnitName}. Attacking!");
                enemy.ExecuteAction<AttackAction>();
                yield return new WaitForSeconds(0.5f);
                yield break;
            }

            // 🔹 Otherwise, move toward the target
            var reachable = Pathfinder.FloodFill(grid, enemy.CurrentCell, enemy.Stats.maxMove);
            GridCell bestCell = enemy.CurrentCell;
            int bestDist = int.MaxValue;

            foreach (var cell in reachable)
            {
                if (cell.Occupant != null && cell != enemy.CurrentCell)
                    continue;

                int d = Mathf.Abs(cell.GridPos.x - target.CurrentCell.GridPos.x) +
                        Mathf.Abs(cell.GridPos.y - target.CurrentCell.GridPos.y);

                if (d < bestDist)
                {
                    bestDist = d;
                    bestCell = cell;
                }
            }

            if (bestCell != enemy.CurrentCell)
            {
                var path = Pathfinder.FindPath(grid, enemy.CurrentCell, bestCell);
                if (path != null)
                {
                    Debug.Log($"[AIController] {enemy.UnitName} moving toward {target.UnitName}.");
                    yield return enemy.StartCoroutine(enemy.MoveAlong(path));
                }
            }

            // 🔹 After moving, attack if now in range
            dist = GridDistance(enemy, target);
            if (dist <= enemy.Stats.attackRange)
            {
                Debug.Log($"[AIController] {enemy.UnitName} attacks {target.UnitName} after moving!");
                enemy.ExecuteAction<AttackAction>();
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                Debug.Log($"[AIController] {enemy.UnitName} cannot reach {target.UnitName} this turn.");
            }

            Debug.Log($"[AIController] {enemy.UnitName} finished its turn.");
        }

        private int GridDistance(Unit a, Unit b)
        {
            return Mathf.Abs(a.CurrentCell.GridPos.x - b.CurrentCell.GridPos.x) +
                   Mathf.Abs(a.CurrentCell.GridPos.y - b.CurrentCell.GridPos.y);
        }
    }
}
