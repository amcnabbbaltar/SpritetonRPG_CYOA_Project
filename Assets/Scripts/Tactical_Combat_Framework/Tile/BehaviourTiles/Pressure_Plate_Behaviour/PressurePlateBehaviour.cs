using System;
using UnityEngine;

namespace Tactics2D
{
    [CreateAssetMenu(menuName = "Tactics2D/Behaviors/Pressure Plate Behavior")]
    public class PressurePlateBehaviour : ScriptableObject, ITileBehavior
    {
        public string group = "A";
        public bool repeat = true;

        public void Initialize(GridManager grid, Vector3Int pos)
        {
            PressurePlateSystem.Instance.RegisterPressurePlate(pos, group);
        }

        public void OnUnitEnter(Unit unit, GridManager grid)
        {
            PressurePlateSystem.Instance.ActivatePressurePlate(unit.CurrentCell.GridPos, group);
        }

        public void OnUnitExit(Unit unit, GridManager grid)
        {
            if (repeat)
                PressurePlateSystem.Instance.DeactivatePressurePlate(unit.CurrentCell.GridPos, group);
        }
    }
}