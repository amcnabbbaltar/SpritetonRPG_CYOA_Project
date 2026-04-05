using System.Collections.Generic;
using UnityEngine;

namespace Tactics2D
{
    public class ActionSystem : MonoBehaviour
    {
        public static ActionSystem Instance { get; private set; }

        private readonly Dictionary<Vector3Int, string> _doorPlates = new(); // tile position -> group
        private readonly Dictionary<Vector3Int, GameObject> _doorOpenEffects = new(); // tile position -> VFX prefab

        // Animator (prefab-based)
        private readonly Dictionary<Vector3Int, Animator> _doorAnimators = new();
        private readonly Dictionary<Vector3Int, string> _doorOpenTriggers = new();
        private readonly Dictionary<Vector3Int, string> _doorCloseTriggers = new();

        // Sprite swap (prefab SpriteRenderer or plain SpriteRenderer)
        private readonly Dictionary<Vector3Int, SpriteRenderer> _doorRenderers = new();
        private readonly Dictionary<Vector3Int, Sprite> _doorClosedSprites = new();
        private readonly Dictionary<Vector3Int, Sprite> _doorOpenSprites = new();

        private GridManager _grid;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        /// <summary>
        /// Clears all dictionaries and initializes the gridManager.
        /// </summary>
        public void Initialize(GridManager gridManager)
        {
            _grid = gridManager;
            _doorPlates.Clear();
            _doorOpenEffects.Clear();
            _doorAnimators.Clear();
            _doorOpenTriggers.Clear();
            _doorCloseTriggers.Clear();
            _doorRenderers.Clear();
            _doorClosedSprites.Clear();
            _doorOpenSprites.Clear();
        }

        /// <summary>
        /// Registers a door tile and spawns its world-space visual.
        /// If a prefab is provided it is instantiated; its Animator and SpriteRenderer are used if present.
        /// If no prefab is provided but sprites are set, a plain SpriteRenderer GameObject is spawned instead.
        /// </summary>
        public void RegisterDoorTile(
            Vector3Int pos, string group, Vector3 worldPos,
            GameObject doorPrefab = null,
            string openTrigger = null, string closeTrigger = null,
            Sprite closedSprite = null, Sprite openSprite = null,
            GameObject openEffect = null)
        {
            _doorPlates.Add(pos, group);
            if (openEffect != null) _doorOpenEffects.Add(pos, openEffect);

            if (doorPrefab != null)
            {
                var instance = Instantiate(doorPrefab, worldPos, Quaternion.identity);

                var animator = instance.GetComponentInChildren<Animator>();
                if (animator != null)
                {
                    _doorAnimators.Add(pos, animator);
                    if (!string.IsNullOrEmpty(openTrigger))  _doorOpenTriggers.Add(pos, openTrigger);
                    if (!string.IsNullOrEmpty(closeTrigger)) _doorCloseTriggers.Add(pos, closeTrigger);
                }

                // Also support sprite swap on the prefab's SpriteRenderer if sprites are assigned
                if (closedSprite != null || openSprite != null)
                {
                    var sr = instance.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.sprite = closedSprite;
                        _doorRenderers.Add(pos, sr);
                        if (closedSprite != null) _doorClosedSprites.Add(pos, closedSprite);
                        if (openSprite != null)   _doorOpenSprites.Add(pos, openSprite);
                    }
                }
            }
            else if (closedSprite != null || openSprite != null)
            {
                // No prefab — spawn a plain SpriteRenderer GameObject
                var go = new GameObject($"Door_Visual_{pos}");
                go.transform.position = worldPos;
                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = closedSprite;
                _doorRenderers.Add(pos, sr);
                if (closedSprite != null) _doorClosedSprites.Add(pos, closedSprite);
                if (openSprite != null)   _doorOpenSprites.Add(pos, openSprite);
            }
        }

        /// <summary>
        /// Opens all doors in the group: toggles walkability, fires the open animator trigger and/or swaps sprite.
        /// </summary>
        public void OpenDoor(string group)
        {
            var doors = GetDoorsInGroup(group);
            doors.ForEach(i => _grid.ToggleWalkable(i));
            doors.ForEach(i =>
            {
                if (_doorAnimators.TryGetValue(i, out var animator) && _doorOpenTriggers.TryGetValue(i, out var trigger))
                    animator.SetTrigger(trigger);

                if (_doorRenderers.TryGetValue(i, out var sr) && _doorOpenSprites.TryGetValue(i, out var sprite))
                    sr.sprite = sprite;

                if (_doorOpenEffects.TryGetValue(i, out var fx))
                    Instantiate(fx, _grid.GetCellPosition(i), Quaternion.identity);
            });
        }

        /// <summary>
        /// Closes all doors in the group: toggles walkability, fires the close animator trigger and/or swaps sprite.
        /// </summary>
        public void CloseDoor(string group)
        {
            var doors = GetDoorsInGroup(group);
            doors.ForEach(i => _grid.ToggleWalkable(i));
            doors.ForEach(i =>
            {
                if (_doorAnimators.TryGetValue(i, out var animator) && _doorCloseTriggers.TryGetValue(i, out var trigger))
                    animator.SetTrigger(trigger);

                if (_doorRenderers.TryGetValue(i, out var sr) && _doorClosedSprites.TryGetValue(i, out var sprite))
                    sr.sprite = sprite;
            });
        }

        private List<Vector3Int> GetDoorsInGroup(string group)
        {
            var doors = new List<Vector3Int>();
            foreach (var kv in _doorPlates)
            {
                if (kv.Value == group)
                    doors.Add(kv.Key);
            }
            return doors;
        }
    }
}
