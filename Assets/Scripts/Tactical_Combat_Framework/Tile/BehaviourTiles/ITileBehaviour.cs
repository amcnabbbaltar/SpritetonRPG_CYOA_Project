using UnityEngine;

namespace Tactics2D
{
    public interface ITileBehavior
    {
        void Initialize(GridManager grid, Vector3Int position);
        void OnUnitEnter(Unit unit, GridManager grid);
        void OnUnitExit(Unit unit, GridManager grid);
    }
}
