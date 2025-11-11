using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics2D
{
    /// <summary>
    /// Handles player input for selecting, moving, and attacking units.
    /// Integrates with the grid, pathfinding, and turn system.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private TurnManager turnManager;
        //[SerializeField] private AllyDetector allyDetector;

        private Unit selectedUnit;
        private HashSet<GridCell> moveRange = new();
        private GridManager grid;

        private void Start()
        {
            grid = GridManager.Instance;
            if (mainCamera == null)
                mainCamera = Camera.main;


            //allyDetector = GameObject.Find(selectedUnit.ToString()).GetComponent<AllyDetector>();

            //if (allyDetector == null)
            //{
            //    Debug.Log("ALLY DETECTOR SCRIPT NOT FOUND.");
            //}
    
}

        private void Update()
        {
            if (!turnManager.IsPlayersTurn) return;
            if (selectedUnit != null && selectedUnit.IsBusy) return;

            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var clickedCell = GetCellUnderMouse();
                if (clickedCell == null) return;

                // Select a friendly unit
                if (clickedCell.Occupant != null && clickedCell.Occupant.Team == Team.Player)
                {
                    SelectUnit(clickedCell.Occupant);
                    selectedUnit.GetComponent<AllyDetector>().FindAllies(); // Detecting allies nearby player clicked on

                }
                // Attack an enemy
                else if (selectedUnit != null &&
                         clickedCell.Occupant != null &&
                         clickedCell.Occupant.Team != selectedUnit.Team)
                {
                    StartCoroutine(PerformAttack(clickedCell.Occupant));

                }
                // Move to empty cell
                else if (selectedUnit != null &&
                         clickedCell.Occupant == null &&
                         moveRange.Contains(clickedCell))
                {
                    StartCoroutine(MoveSelectedUnit(clickedCell));

                }
            }

            // Right-click to deselect
            if (Input.GetMouseButtonDown(1))
                ClearSelection();
        }

        #region Selection
        private void SelectUnit(Unit unit)
        {
            if (unit == null || !unit.IsAlive) return;

            selectedUnit = unit;

            moveRange = Pathfinder.FloodFill(
                grid,
                unit.CurrentCell,
                unit.Stats.maxMove,
                grid.GetCost
            );

            grid.Highlight(moveRange);

            
        }

        private void ClearSelection()
        {
            selectedUnit = null;
            moveRange.Clear();
            grid.ClearHighlight();
        }
        #endregion

        #region Actions
        private IEnumerator MoveSelectedUnit(GridCell destination)
        {
            if (selectedUnit == null) yield break;

            var path = Pathfinder.FindPath(grid, selectedUnit.CurrentCell, destination, grid.GetCost);
            if (path == null)
            {
                Debug.LogWarning("No valid path found.");
                yield break;
            }

            grid.ClearHighlight();
            yield return StartCoroutine(selectedUnit.MoveAlong(path));

            turnManager.EndTurn();
            ClearSelection();
        }

        private IEnumerator PerformAttack(Unit target)
        {
            if (selectedUnit == null || target == null)
                yield break;

            // Find attack action in this unit’s list
            foreach (var action in selectedUnit.GetActions())
            {
                if (action is AttackAction attackAction && action.CanExecute(selectedUnit))
                {
                    yield return selectedUnit.StartCoroutine(action.Execute(selectedUnit));
                    break;
                }
            }

            turnManager.EndTurn();
            ClearSelection();
        }

        public void ForceEndTurn()
        {
            ClearSelection();
            turnManager.EndTurn();
        }
        #endregion

        #region Utility
        private GridCell GetCellUnderMouse()
        {
            Vector3 world = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            return grid.CellFromWorld(world);
        }
        #endregion
    }
}
