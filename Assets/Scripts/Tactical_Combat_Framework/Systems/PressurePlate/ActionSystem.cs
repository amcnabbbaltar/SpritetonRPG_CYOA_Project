using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics2D
{
    public class ActionSystem : MonoBehaviour
    {
        public static ActionSystem Instance { get; private set; }
        private readonly Dictionary<string, Vector3Int> doorPlates = new(); // tile -> group
        private readonly Dictionary<string, int> doorGroupsStatus = new(); // group -> true | false

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
            doorPlates.Clear();
            doorGroupsStatus.Clear();
        }

        public void OpenDoor(string group)
        {
            grid.Cells
            doorPlates[group]
        }

        public void RegisterDoorTile(Vector3Int pos, string group)
        {
            doorPlates.Add(group, pos);
        }
    }
}