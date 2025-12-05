using System;
using UnityEngine;

namespace Tactics2D
{
    [CreateAssetMenu(menuName = "Tactics2D/Behaviors/Pressure Plate Behavior")]
    public class PressurePlateBehaviour : ScriptableObject, ITileBehavior
    {
        [Header("Pressure Plate Settings")]
        [SerializeField] public string group = "A";
        [SerializeField] public bool reset = true;

        private bool _activated = false;
        
        /// <summary>
        /// Register Pressure Plate in System
        /// </summary>
        /// <param name="grid">Grid manager</param>
        /// <param name="pos">Pressure plate position</param>
        public void Initialize(GridManager grid, Vector3Int pos)
        {
            PressurePlateSystem.Instance.RegisterPressurePlate(pos, group);
        }

        /// <summary>
        /// Activate Pressure Plate System when entered
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <param name="grid">Grid manager</param>
        public void OnUnitEnter(Unit unit, GridManager grid)
        {
            if (reset || !_activated)
            {
                _activated = true;
                PressurePlateSystem.Instance.ActivatePressurePlate(unit.CurrentCell.GridPos, group);
            }
        }

        /// <summary>
        /// Deactivate  Pressure Plate System when leaves
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <param name="grid">Grid manager</param>
        public void OnUnitExit(Unit unit, GridManager grid)
        {
            if (reset)
                PressurePlateSystem.Instance.DeactivatePressurePlate(unit.CurrentCell.GridPos, group);
        }
    }
}