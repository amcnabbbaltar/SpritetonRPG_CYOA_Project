using UnityEngine;

namespace Tactics2D
{
    [CreateAssetMenu(menuName = "Tactics2D/Behaviors/Teleport Behavior")]
    public class TeleportBehavior : ScriptableObject, ITileBehavior
    {
        public string group = "A";
        public GameObject teleportEffect;

        public void Initialize(GridManager grid, Vector3Int pos)
        {
            TeleportSystem.Instance.RegisterTeleport(pos, group, teleportEffect);
        }

        public void OnUnitEnter(Unit unit, GridManager grid)
        {
            var cell = grid.CellFromWorld(unit.transform.position);
            TeleportSystem.Instance.TryTeleport(unit, cell);
        }

        public void OnUnitExit(Unit unit, GridManager grid) { }
    }
}
