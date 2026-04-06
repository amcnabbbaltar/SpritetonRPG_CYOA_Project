using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tactics2D
{
    public enum ObjectiveType { KillAllEnemies, ReachTile, SolvePlateGroup, TalkToNPC }

    [Serializable]
    public class BattleObjective
    {
        [Tooltip("Text shown in the HUD, e.g. \"Defeat all enemies\"")]
        public string description;
        public ObjectiveType type;
        [Tooltip("Required for ReachTile, SolvePlateGroup, TalkToNPC — must match the id used when firing the event")]
        public string targetId;
        [HideInInspector] public bool isComplete;
    }

    /// <summary>
    /// Listens to TacticalEvents and marks objectives complete.
    /// Add this component to any persistent GameObject in the battle scene
    /// and populate the objectives list in the Inspector.
    /// </summary>
    public class ObjectiveManager : MonoBehaviour
    {
        [SerializeField] private List<BattleObjective> objectives = new();

        private void OnEnable()
        {
            TacticalEvents.OnUnitKilled              += OnUnitKilled;
            TacticalEvents.OnObjectiveTileReached    += OnTileReached;
            TacticalEvents.OnPressurePlateGroupSolved += OnGroupSolved;
            TacticalEvents.OnNPCTalkedTo             += OnNPCTalked;
        }

        private void OnDisable()
        {
            TacticalEvents.OnUnitKilled              -= OnUnitKilled;
            TacticalEvents.OnObjectiveTileReached    -= OnTileReached;
            TacticalEvents.OnPressurePlateGroupSolved -= OnGroupSolved;
            TacticalEvents.OnNPCTalkedTo             -= OnNPCTalked;
        }

        private void Start() => RefreshHUD();

        // ── Event handlers ─────────────────────────────────────────────────────

        private void OnUnitKilled(Unit _)
        {
            bool anyEnemyAlive = FindObjectsOfType<Unit>()
                .Any(u => u.Team == Team.Enemy && u.IsAlive);

            foreach (var obj in objectives)
                if (!obj.isComplete && obj.type == ObjectiveType.KillAllEnemies && !anyEnemyAlive)
                    Complete(obj);
        }

        private void OnTileReached(string id)
        {
            foreach (var obj in objectives)
                if (!obj.isComplete && obj.type == ObjectiveType.ReachTile && obj.targetId == id)
                    Complete(obj);
        }

        private void OnGroupSolved(string group)
        {
            foreach (var obj in objectives)
                if (!obj.isComplete && obj.type == ObjectiveType.SolvePlateGroup && obj.targetId == group)
                    Complete(obj);
        }

        private void OnNPCTalked(string npcId)
        {
            foreach (var obj in objectives)
                if (!obj.isComplete && obj.type == ObjectiveType.TalkToNPC && obj.targetId == npcId)
                    Complete(obj);
        }

        // ── Internal ───────────────────────────────────────────────────────────

        private void Complete(BattleObjective obj)
        {
            obj.isComplete = true;
            Debug.Log($"[ObjectiveManager] Objective complete: {obj.description}");
            RefreshHUD();
        }

        private void RefreshHUD() => HUDController.Instance?.SetObjectives(objectives);
    }
}
