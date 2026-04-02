using UnityEngine;

namespace Tactics2D
{
    [CreateAssetMenu(menuName = "Tactics2D/Behaviors/Door Behavior")]
    public class DoorBehaviour : ScriptableObject, ITileBehavior
    {
        [Header("Door Settings")]
        [SerializeField] public string group = "A";
        [Tooltip("VFX prefab instantiated once at the door's position when the door opens.")]
        [SerializeField] public GameObject openEffect;

        [Header("Visual Prefab")]
        [Tooltip("Prefab spawned in the world at this tile. Requires an Animator for trigger-based animation.")]
        [SerializeField] public GameObject doorPrefab;
        [SerializeField] public string openTrigger = "Open";
        [SerializeField] public string closeTrigger = "Close";

        [Header("Sprite Swap")]
        [Tooltip("Used when no prefab is assigned (spawns a plain SpriteRenderer), or alongside a prefab that has a SpriteRenderer.")]
        [SerializeField] public Sprite closedSprite;
        [SerializeField] public Sprite openSprite;

        /// <summary>
        /// Register Door in Action System
        /// </summary>
        public void Initialize(GridManager grid, Vector3Int position)
        {
            ActionSystem.Instance.RegisterDoorTile(
                position, group, grid.GetCellPosition(position),
                doorPrefab, openTrigger, closeTrigger,
                closedSprite, openSprite, openEffect);
        }

        /// <summary>
        /// [Does Nothing] Doors are controlled by the ActionSystem via pressure plates
        /// </summary>
        public void OnUnitEnter(Unit unit, GridManager grid) { }

        /// <summary>
        /// [Does Nothing] Doors are controlled by the ActionSystem via pressure plates
        /// </summary>
        public void OnUnitExit(Unit unit, GridManager grid) { }
    }
}
