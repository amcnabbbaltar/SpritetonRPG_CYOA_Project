using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics2D
{
    public class ActionSystem : MonoBehaviour
    {
        public static ActionSystem Instance { get; private set; }
        private readonly Dictionary<Vector3Int, string> _doorPlates = new(); // tile position -> group
        private readonly Dictionary<Vector3Int, GameObject> _doorEffects = new(); // tile position -> effect

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

        /// <summary>
        /// Clears all dictionaries and initializes the gridManager
        /// </summary>
        /// <param name="gridManager"></param>
        public void Initialize(GridManager gridManager)
        {
            _grid = gridManager;
            _doorPlates.Clear();
        }

        /// <summary>
        /// Open door linked to specific group
        /// </summary>
        /// <param name="group">Door group</param>
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
            doors.ForEach(i =>
            {
                print($"[Door Effect] Maybe ${i} : ${_grid.GetCellPosition(i)} but {_doorEffects.ContainsKey(i)}");
                if (_doorEffects.TryGetValue(i, out var fx))
                {
                    print($"[Door Effect]{i}");
                    Instantiate(fx, _grid.GetCellPosition(i), Quaternion.identity);
                }
            });
        }
        
        /// <summary>
        /// Close door linked to specific group
        /// </summary>
        /// <param name="group">Door group</param>
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

        /// <summary>
        /// Adds the door to the dictionaries
        /// </summary>
        /// <param name="pos">Door position</param>
        /// <param name="group">Door group</param>
        public void RegisterDoorTile(Vector3Int pos, string group, GameObject fx = null)
        {
            _doorPlates.Add(pos, group);
            if (fx)
                _doorEffects.Add(pos, fx);
        }
    }
}