using System.Collections;
using System.Collections.Generic;
using Tactics2D;
using UnityEngine;

namespace Tactics2D
{
    [CreateAssetMenu(menuName = "Tactics2D/Behaviors/Door Behavior")]
    public class DoorBehaviour : ScriptableObject, ITileBehavior
    {
        [Header("Door Settings")]
        [SerializeField] public string group = "A";
        [SerializeField] public GameObject doorEffect;

        /// <summary>
        /// Register Door in Action System
        /// </summary>
        /// <param name="grid">Grid manager</param>
        /// <param name="pos">Pressure plate position</param>
        public void Initialize(GridManager grid, Vector3Int position)
        {
            ActionSystem.Instance.RegisterDoorTile(position, group, doorEffect);
        }

        /// <summary>
        /// [Does Nothing] Activate Door in Action System when entered
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <param name="grid">Grid manager</param>
        public void OnUnitEnter(Unit unit, GridManager grid)
        {
            
        }

        /// <summary>
        /// [Does Nothing] Deactivate Door in Action System when leaves
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <param name="grid">Grid manager</param>
        public void OnUnitExit(Unit unit, GridManager grid)
        {
            
        }
    }
}