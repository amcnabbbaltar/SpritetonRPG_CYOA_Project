
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

        private readonly Dictionary<Vector3Int, string> pressurePlates = new(); // tile -> group
        private readonly Dictionary<Vector3Int, bool> pressurePlatesStatus = new(); // pos -> true | false
        private readonly Dictionary<string, int> pressurePlateGroups = new(); // group -> count
        private readonly Dictionary<string, bool> pressurePlateGroupsStatus = new(); // group -> true | false

        private GridManager grid;

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
        /// 
        /// </summary>
        /// <param name="gridManager"></param>
        public void Initialize(GridManager gridManager)
        {
            grid = gridManager;
            pressurePlates.Clear();
            pressurePlatesStatus.Clear();
            pressurePlateGroups.Clear();
            pressurePlateGroupsStatus.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="group"></param>
        /// <param name="action"></param>
        public void ActivatePressurePlate(Vector3Int pos, string group)
        {
            pressurePlatesStatus[pos] = true;
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
                // DO SOMETHING
                Debug.Log($"[PressurePlatesSystem] Trigger {group}");
            }
            Debug.Log($"[PressurePlatesSystem] Comes {group}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="group"></param>
        /// <param name="action"></param>
        public void DeactivatePressurePlate(Vector3Int pos, string group)
        {
            if (pressurePlateGroupsStatus[group])
            {
                Debug.Log($"[PressurePlatesSystem] Group Bye Bye {group}");
            }

            pressurePlatesStatus[pos] = false;
            Debug.Log($"[PressurePlatesSystem] Leaves {group}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="group"></param>
        public void RegisterPressurePlate(Vector3Int pos, string group)
        {
            pressurePlates.Add(pos, group);
            pressurePlatesStatus.Add(pos, false);
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
    }
}