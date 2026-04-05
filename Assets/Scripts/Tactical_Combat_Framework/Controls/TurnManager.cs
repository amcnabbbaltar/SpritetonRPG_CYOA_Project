using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tactics2D
{
    /// <summary>
    /// Manages player and enemy turns. Each player unit must call UnitEndedTurn()
    /// before the enemy phase begins. Delegates enemy behavior to AIController.
    /// </summary>
    public class TurnManager : MonoBehaviour
    {
        public bool IsPlayersTurn { get; private set; } = true;

        private List<Unit> playerUnits = new();
        private List<Unit> enemyUnits = new();

        // Snapshot of units that still need to act this player phase.
        // Rebuilt at the start of every player phase so it is never stale.
        private HashSet<Unit> _pendingUnits = new();

        private AIController aiController;

        private void Start()
        {
            playerUnits = FindObjectsOfType<Unit>().Where(u => u.Team == Team.Player).ToList();
            enemyUnits  = FindObjectsOfType<Unit>().Where(u => u.Team == Team.Enemy).ToList();
            aiController = FindObjectOfType<AIController>();

            Debug.Log($"[TurnManager] {playerUnits.Count} players, {enemyUnits.Count} enemies.");
            BeginPlayerPhase();
        }

        /// <summary>
        /// Call this when a player unit finishes its actions.
        /// The enemy phase starts automatically once every pending unit has acted.
        /// </summary>
        public void UnitEndedTurn(Unit unit)
        {
            if (!IsPlayersTurn) return;

            _pendingUnits.Remove(unit);

            int acted = playerUnits.Count - _pendingUnits.Count;
            Debug.Log($"[TurnManager] {unit.name} ended turn. ({acted}/{playerUnits.Count} acted)");
            HUDController.Instance?.SetActedProgress(acted, playerUnits.Count);

            if (_pendingUnits.Count == 0)
                StartCoroutine(EnemyPhase());
        }

        /// <summary>
        /// Immediately ends the player phase and starts the enemy phase.
        /// Use this for an "End Phase" UI button.
        /// </summary>
        public void EndTurn()
        {
            if (!IsPlayersTurn) return;
            Debug.Log("[TurnManager] Player phase force-ended via EndTurn().");
            StartCoroutine(EnemyPhase());
        }

        private IEnumerator EnemyPhase()
        {
            IsPlayersTurn = false;
            HUDController.Instance?.SetPhase(false);

            enemyUnits.RemoveAll(e => e == null || !e.IsAlive);
            HUDController.Instance?.SetEnemyCount(enemyUnits.Count);

            if (enemyUnits.Count == 0)
            {
                Debug.Log("[TurnManager] All enemies defeated!");
                SceneManager.LoadScene("Town_Exemple");
                yield break;
            }

            Debug.Log("[TurnManager] Enemy phase starting...");

            if (aiController == null)
            {
                Debug.LogError("[TurnManager] Missing AIController reference!");
            }
            else
            {
                foreach (var enemy in enemyUnits.ToList())
                {
                    if (enemy == null || !enemy.IsAlive) continue;
                    yield return aiController.ExecuteTurn(enemy);
                }
            }

            Debug.Log("[TurnManager] Enemy phase complete. Player phase starting...");

            playerUnits.RemoveAll(p => p == null || !p.IsAlive);
            HUDController.Instance?.NextRound();
            BeginPlayerPhase();
        }

        /// <summary>
        /// Snapshot the currently alive player units and hand control back to the player.
        /// </summary>
        private void BeginPlayerPhase()
        {
            _pendingUnits = new HashSet<Unit>(playerUnits);
            IsPlayersTurn = true;
            RefreshHUD();
        }

        private void RefreshHUD()
        {
            HUDController.Instance?.SetPhase(IsPlayersTurn);
            HUDController.Instance?.SetActedProgress(0, playerUnits.Count);
            HUDController.Instance?.SetEnemyCount(enemyUnits.Count(e => e != null && e.IsAlive));
        }
    }
}
