using System.Collections.Generic;
using UnityEngine;

namespace Tactics2D
{
    public class PressurePlateSystem : MonoBehaviour
    {
        public static PressurePlateSystem Instance { get; private set; }

        private readonly Dictionary<Vector3Int, string> pressurePlates = new(); // tile pos -> group
        private readonly Dictionary<Vector3Int, bool> pressurePlatesStatus = new(); // tile pos -> active state
        private readonly Dictionary<string, int> pressurePlateGroups = new(); // group -> total count
        private readonly Dictionary<string, bool> pressurePlateGroupsStatus = new(); // group -> active state

        // Animator (prefab-based)
        private readonly Dictionary<Vector3Int, Animator> pressurePlateAnimators = new();
        private readonly Dictionary<Vector3Int, string> pressurePlateActivateTriggers = new();
        private readonly Dictionary<Vector3Int, string> pressurePlateDeactivateTriggers = new();

        // Sprite swap (prefab SpriteRenderer or plain SpriteRenderer)
        private readonly Dictionary<Vector3Int, SpriteRenderer> pressurePlateRenderers = new();
        private readonly Dictionary<Vector3Int, Sprite> pressurePlateActiveSprites = new();
        private readonly Dictionary<Vector3Int, Sprite> pressurePlateInactiveSprites = new();

        private GridManager grid;
        private IPressurePlateInterpreter pressurePlateInterpreter;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        /// <summary>
        /// Clears all dictionaries and initializes the gridManager and interpreter.
        /// </summary>
        public void Initialize(GridManager gridManager)
        {
            pressurePlateInterpreter = GetComponent<IPressurePlateInterpreter>();
            grid = gridManager;
            pressurePlates.Clear();
            pressurePlatesStatus.Clear();
            pressurePlateGroups.Clear();
            pressurePlateGroupsStatus.Clear();
            pressurePlateAnimators.Clear();
            pressurePlateActivateTriggers.Clear();
            pressurePlateDeactivateTriggers.Clear();
            pressurePlateRenderers.Clear();
            pressurePlateActiveSprites.Clear();
            pressurePlateInactiveSprites.Clear();
        }

        /// <summary>
        /// Registers a pressure plate and spawns its world-space visual.
        /// If a prefab is provided it is instantiated; its Animator and SpriteRenderer are used if present.
        /// If no prefab is provided but sprites are set, a plain SpriteRenderer GameObject is spawned instead.
        /// </summary>
        public void RegisterPressurePlate(
            Vector3Int pos, string group, Vector3 worldPos,
            GameObject visualPrefab = null,
            string activateTrigger = null, string deactivateTrigger = null,
            Sprite inactiveSprite = null, Sprite activeSprite = null)
        {
            pressurePlates.Add(pos, group);
            pressurePlatesStatus.Add(pos, false);

            if (visualPrefab != null)
            {
                var instance = Instantiate(visualPrefab, worldPos, Quaternion.identity);

                var animator = instance.GetComponentInChildren<Animator>();
                if (animator != null)
                {
                    pressurePlateAnimators.Add(pos, animator);
                    if (!string.IsNullOrEmpty(activateTrigger))   pressurePlateActivateTriggers.Add(pos, activateTrigger);
                    if (!string.IsNullOrEmpty(deactivateTrigger)) pressurePlateDeactivateTriggers.Add(pos, deactivateTrigger);
                }

                // Also support sprite swap on the prefab's SpriteRenderer if sprites are assigned
                if (inactiveSprite != null || activeSprite != null)
                {
                    var sr = instance.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.sprite = inactiveSprite;
                        pressurePlateRenderers.Add(pos, sr);
                        if (inactiveSprite != null) pressurePlateInactiveSprites.Add(pos, inactiveSprite);
                        if (activeSprite != null)   pressurePlateActiveSprites.Add(pos, activeSprite);
                    }
                }
            }
            else if (inactiveSprite != null || activeSprite != null)
            {
                // No prefab — spawn a plain SpriteRenderer GameObject
                var go = new GameObject($"PressurePlate_Visual_{pos}");
                go.transform.position = worldPos;
                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = inactiveSprite;
                pressurePlateRenderers.Add(pos, sr);
                if (inactiveSprite != null) pressurePlateInactiveSprites.Add(pos, inactiveSprite);
                if (activeSprite != null)   pressurePlateActiveSprites.Add(pos, activeSprite);
            }

            if (!pressurePlateGroups.ContainsKey(group))
            {
                pressurePlateGroupsStatus.Add(group, false);
                pressurePlateGroups.Add(group, 1);
            }
            else
            {
                pressurePlateGroups[group] += 1;
            }
        }

        /// <summary>
        /// Activates a pressure plate: fires its animator trigger and/or swaps its sprite.
        /// If all plates in the group are active, fires the interpreter.
        /// </summary>
        public void ActivatePressurePlate(Vector3Int pos, string group)
        {
            pressurePlatesStatus[pos] = true;

            if (pressurePlateAnimators.TryGetValue(pos, out var animator) && pressurePlateActivateTriggers.TryGetValue(pos, out var animTrigger))
                animator.SetTrigger(animTrigger);

            if (pressurePlateRenderers.TryGetValue(pos, out var sr) && pressurePlateActiveSprites.TryGetValue(pos, out var activeSprite))
                sr.sprite = activeSprite;

            int count = 0;
            foreach (var plate in pressurePlates)
            {
                if (plate.Value == group && pressurePlatesStatus[plate.Key])
                    count++;
            }

            if (pressurePlateGroups[group] == count)
            {
                pressurePlateGroupsStatus[group] = true;
                pressurePlateInterpreter.ActivateTrigger(group);
            }
        }

        /// <summary>
        /// Deactivates a pressure plate: fires its animator trigger and/or swaps its sprite back.
        /// If the group was active, fires the interpreter deactivation.
        /// </summary>
        public void DeactivatePressurePlate(Vector3Int pos, string group)
        {
            if (pressurePlateGroupsStatus[group])
                pressurePlateInterpreter.DeactivateTrigger(group);

            pressurePlatesStatus[pos] = false;

            if (pressurePlateAnimators.TryGetValue(pos, out var animator) && pressurePlateDeactivateTriggers.TryGetValue(pos, out var animTrigger))
                animator.SetTrigger(animTrigger);

            if (pressurePlateRenderers.TryGetValue(pos, out var sr) && pressurePlateInactiveSprites.TryGetValue(pos, out var inactiveSprite))
                sr.sprite = inactiveSprite;
        }
    }
}
