using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics2D
{
    public class ActionSystem : MonoBehaviour
    {
        public static ActionSystem Instance { get; private set; }
        private readonly Dictionary<Vector3Int, string> _doorPlates = new(); // tile -> group

        private GridManager _grid;

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
            _grid = gridManager;
            _doorPlates.Clear();
        }

        public void OpenDoor(string group)
        {
            List<Vector3Int> doors = new();
            
            foreach (KeyValuePair<Vector3Int, string> value in _doorPlates)
            {
                if (value.Value == group)
                {
                    doors.Add(value.Key);
                }
            }
            doors.ForEach(i => _grid.ToggleWalkable(i));
        }
        
        public void CloseDoor(string group)
        {
            List<Vector3Int> doors = new();
            
            foreach (KeyValuePair<Vector3Int, string> value in _doorPlates)
            {
                if (value.Value == group)
                {
                    doors.Add(value.Key);
                }
            }
            doors.ForEach(i => _grid.ToggleWalkable(i));
        }

        public void RegisterDoorTile(Vector3Int pos, string group)
        {
            _doorPlates.Add(pos, group);
        }
    }
}