using UnityEngine;

namespace Tactics2D
{
    [CreateAssetMenu(menuName = "Tactics2D/Behaviors/Pressure Plate Behavior")]
    public class PressurePlateBehaviour : ScriptableObject, ITileBehavior
    {
        [Header("Pressure Plate Settings")]
        [SerializeField] public string group = "A";
        [SerializeField] public bool reset = true;

        [Header("Visual Prefab")]
        [Tooltip("Prefab spawned in the world at this tile. Requires an Animator for trigger-based animation.")]
        [SerializeField] public GameObject visualPrefab;
        [SerializeField] public string activateTrigger = "Activate";
        [SerializeField] public string deactivateTrigger = "Deactivate";

        [Header("Sprite Swap")]
        [Tooltip("Used when no prefab is assigned (spawns a plain SpriteRenderer), or alongside a prefab that has a SpriteRenderer.")]
        [SerializeField] public Sprite inactiveSprite;
        [SerializeField] public Sprite activeSprite;

        private bool _activated = false;

        /// <summary>
        /// Register Pressure Plate in System
        /// </summary>
        public void Initialize(GridManager grid, Vector3Int pos)
        {
            PressurePlateSystem.Instance.RegisterPressurePlate(
                pos, group, grid.GetCellPosition(pos),
                visualPrefab, activateTrigger, deactivateTrigger,
                inactiveSprite, activeSprite);
        }

        /// <summary>
        /// Activate Pressure Plate System when entered
        /// </summary>
        public void OnUnitEnter(Unit unit, GridManager grid)
        {
            if (reset || !_activated)
            {
                _activated = true;
                PressurePlateSystem.Instance.ActivatePressurePlate(unit.CurrentCell.GridPos, group);
            }
        }

        /// <summary>
        /// Deactivate Pressure Plate System when leaves
        /// </summary>
        public void OnUnitExit(Unit unit, GridManager grid)
        {
            if (reset)
                PressurePlateSystem.Instance.DeactivatePressurePlate(unit.CurrentCell.GridPos, group);
        }
    }
}
