using System.Collections;
using System.Collections.Generic;
using Tactics2D;
using UnityEngine;

namespace Tactics2D
{
    [CreateAssetMenu(menuName = "Tactics2D/Behaviors/Door Behavior")]
    public class DoorBehaviour : ScriptableObject, ITileBehavior
    {
        public string group = "A";

        public void Initialize(GridManager grid, Vector3Int position)
        {
            ActionSystem.Instance.RegisterDoorTile(position, group);
        }

        public void OnUnitEnter(Unit unit, GridManager grid)
        {
            
        }

        public void OnUnitExit(Unit unit, GridManager grid)
        {
            
        }
    }
}