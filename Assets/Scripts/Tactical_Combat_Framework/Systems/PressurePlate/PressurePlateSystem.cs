
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Tactics2D
{
    /// <summary>
    ///     
    /// </summary>
    public class PressurePlateSystem : MonoBehaviour
    {
        public static PressurePlateSystem Instance { get; private set; }

        private readonly Dictionary<Vector3Int, string> pressurePlates = new(); // tile pos -> group
        private readonly Dictionary<Vector3Int, bool> pressurePlatesStatus = new(); // tile pos -> true | false
        private readonly Dictionary<string, int> pressurePlateGroups = new(); // group -> count
        private readonly Dictionary<string, bool> pressurePlateGroupsStatus = new(); // group -> true | false
        private readonly Dictionary<Vector3Int, GameObject> pressurePlateFx = new(); // tile pos -> effect
        
        private GridManager grid;
        private IPressurePlateInterpreter pressurePlateInterpreter; // Interpreter for the actions
        
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Clears all dictionaries and initializes the gridManager and interpreter
        /// </summary>
        /// <param name="gridManager"></param>
        public void Initialize(GridManager gridManager)
        {
            pressurePlateInterpreter = GetComponent<IPressurePlateInterpreter>();
            grid = gridManager;
            pressurePlates.Clear();
            pressurePlatesStatus.Clear();
            pressurePlateGroups.Clear();
            pressurePlateGroupsStatus.Clear();
        }

        /// <summary>
        /// Adds the pressure plate to the dictionaries
        /// </summary>
        /// <param name="pos">Pressure plate position</param>
        /// <param name="group">Pressure plate group</param>
        /// <param name="fx">Pressure plate effect</param>
        public void RegisterPressurePlate(Vector3Int pos, string group, GameObject fx = null)
        {
            pressurePlates.Add(pos, group);
            pressurePlatesStatus.Add(pos, false);
            if (fx)
                pressurePlateFx.Add(pos, fx);
            if (!pressurePlateGroups.ContainsKey(group))
            {
                pressurePlateGroupsStatus.Add(group, false);
                pressurePlateGroups.Add(group, 1);
            }
            else
            {
                pressurePlateGroups[group] += 1;
            }
        }
        
        /// <summary>
        /// Turns on the given pressure plate position of that group.
        /// If all pressure plates in that group are active, activate the interpreter.
        /// </summary>
        /// <param name="pos">Pressure plate position</param>
        /// <param name="group">Pressure plate group</param>
        public void ActivatePressurePlate(Vector3Int pos, string group)
        {
            pressurePlatesStatus[pos] = true;

            if (pressurePlateFx.TryGetValue(pos, out var fx))
                Instantiate(fx, grid.GetCellPosition(pos), Quaternion.identity);
            int count = 0;
            foreach (var pressurePlate in pressurePlates)
            {
                if (pressurePlate.Value == group && pressurePlatesStatus[pressurePlate.Key])
                {
                    count += 1;
                }
            }

            if (pressurePlateGroups[group] == count)
            {
                pressurePlateGroupsStatus[group] = true;
                pressurePlateInterpreter.ActivateTrigger(group);
            }
        }

        /// <summary>
        /// Turns off the given pressure plate position of that group.
        /// If the group of pressure plates was active, deactivate the interpreter.
        /// </summary>
        /// <param name="pos">Pressure plate position</param>
        /// <param name="group">Pressure plate group</param>
        public void DeactivatePressurePlate(Vector3Int pos, string group)
        {
            if (pressurePlateGroupsStatus[group])
            {
                pressurePlateInterpreter.DeactivateTrigger(group);
            }

            pressurePlatesStatus[pos] = false;
        }


    }
}