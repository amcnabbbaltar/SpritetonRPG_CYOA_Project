using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tactics2D
{
    /// <summary>
    /// Manages player and enemy turns. Delegates enemy behavior to AIController.
    /// </summary>
    public class TurnManager : MonoBehaviour
    {
        public bool IsPlayersTurn { get; private set; } = true;

        private List<Unit> playerUnits;
        private List<Unit> enemyUnits;
        private AIController aiController;

        private int playerCount;
        private int enemyCount;

        private void Start()
        {
            playerUnits = FindObjectsOfType<Unit>().Where(u => u.Team == Team.Player).ToList();
            enemyUnits = FindObjectsOfType<Unit>().Where(u => u.Team == Team.Enemy).ToList();
            aiController = FindObjectOfType<AIController>();

            foreach (var enemy in enemyUnits.ToList())
            {
                enemyCount++;
            }

            //foreach (var player in playerUnits.ToList())
            //{
            //    playerCount++;
            //}

            Debug.Log("STARTING ENEMIES: " + enemyCount);
        }
        public void EndTurn()
        {
            
            if (IsPlayersTurn)
            {
                Debug.Log("[TurnManager] Player turn ended. Enemy phase starting...");
                IsPlayersTurn = false;
                StartCoroutine(EnemyPhase());
            }
            else
            {
                Debug.Log("[TurnManager] Enemy turn ended. Back to player!");
                IsPlayersTurn = true;
            }

            foreach (var enemy in enemyUnits.ToList())
            {
                if (enemy == null || !enemy.IsAlive)
                {
                    enemyCount--;
                    enemyUnits.Remove(enemy);
                    Debug.Log("REMAINING ENEMIES: " + enemyCount);

                    if(enemyCount == 0)
                    {
                        Debug.Log("All enemies are dead!");
                        SceneManager.LoadScene("Town_Exemple");
                    }
                }
            }
        }

        private IEnumerator EnemyPhase()
        {
            if (aiController == null)
            {
                Debug.LogError("[TurnManager] Missing AIController reference!");
                yield break;
            }

            // Let AI controller run each enemy’s turn
            foreach (var enemy in enemyUnits.ToList())
            {
                if (enemy == null || !enemy.IsAlive) continue;
                yield return aiController.ExecuteTurn(enemy);
            }

            Debug.Log("[TurnManager] Enemy phase complete!");
            IsPlayersTurn = true;
        }
    }
}
