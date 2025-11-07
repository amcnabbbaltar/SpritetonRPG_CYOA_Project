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

        private readonly Dictionary<Vector3Int, string> pressurePlateGroups = new(); // tile -> group
        private readonly Dictionary<Vector3Int, bool> pressurePlatesActivated = new(); // tile -> group

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

        public void Initialize(GridManager gridManager)
        {
            grid = gridManager;
            pressurePlateGroups.Clear();
            pressurePlatesActivated.Clear();
        }

        public void ActivatePressurePlate(string group)
        {
            Dictionary<Vector3Int, string> pressurePlates = new();
            bool allActive = true;
            foreach (var pressurePlateGroup in pressurePlateGroups)
            {
                if (pressurePlateGroup.Value == group)
                {
                    if (pressurePlatesActivated[pressurePlateGroup.Key])
                    {
                        allActive = false;
                    }
                    pressurePlates.Add(pressurePlateGroup.Key, pressurePlateGroup.Value);
                }
            }
            
            // pressurePlates = pressurePlateGroups.ContainsValue(group);
        }

        public void RegisterPressurePlate(Vector3Int pos, string group)
        {
            pressurePlateGroups[pos] = group;

            if (!pressurePlatesActivated.ContainsKey(pos))
            {
                pressurePlatesActivated.Add(pos, false);
            }
        }
    }
}