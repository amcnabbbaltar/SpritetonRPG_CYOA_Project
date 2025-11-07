using System;
using UnityEngine;

namespace Tactics2D
{
    [CreateAssetMenu(menuName = "Tactics2D/Behaviors/Pressure Plate Behavior")]
    public class PressurePlateBehaviour : ScriptableObject, ITileBehavior
    {
        public string group = "A";
        public bool isActive = false;

        public void Initialize(GridManager grid, Vector3Int pos)
        {
            PressurePlateSystem.Instance.RegisterPressurePlate(pos, group);
            Debug.Log("Meow");
        }

        public void OnUnitEnter(Unit unit, GridManager grid)
        {
            isActive = true;
            
            Debug.Log("You stepped on me oh no!");
        }

        public void OnUnitExit(Unit unit, GridManager grid)
        {
            isActive = false;
            Debug.Log("No Please don't leave T^T");
        }
    }
}