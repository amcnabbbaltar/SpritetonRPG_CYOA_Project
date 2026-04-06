using System;

namespace Tactics2D
{
    /// <summary>
    /// Lightweight static event hub for the objectives system.
    /// Fire from game systems; subscribe from objectives.
    /// </summary>
    public static class TacticalEvents
    {
        /// <summary>Fired when any unit dies. Passes the dead unit.</summary>
        public static event Action<Unit> OnUnitKilled;

        /// <summary>Fired when all plates in a pressure-plate group are activated.</summary>
        public static event Action<string> OnPressurePlateGroupSolved;

        /// <summary>Fired when a unit steps on an ObjectiveTile. Passes the tile's objectiveId.</summary>
        public static event Action<string> OnObjectiveTileReached;

        /// <summary>Fired when a player interacts with an NPC. Passes the NPC's id.</summary>
        public static event Action<string> OnNPCTalkedTo;

        public static void UnitKilled(Unit unit)               => OnUnitKilled?.Invoke(unit);
        public static void GroupSolved(string group)           => OnPressurePlateGroupSolved?.Invoke(group);
        public static void ObjectiveTileReached(string id)     => OnObjectiveTileReached?.Invoke(id);
        public static void NPCTalkedTo(string npcId)           => OnNPCTalkedTo?.Invoke(npcId);
    }
}
