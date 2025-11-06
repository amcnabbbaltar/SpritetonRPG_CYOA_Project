using System.Collections.Generic;
using UnityEngine;

namespace Tactics2D
{
    public class GridCell
    {
        public Vector3Int GridPos { get; private set; }
        public Vector3 WorldCenter { get; private set; }

        public bool Walkable { get; set; } = true;
        public bool BlocksVision { get; set; } = false;

        public List<Unit> Occupants { get; private set; } = new();
        public bool IsTaken => Occupants.Count > 0;
        public Unit Occupant => Occupants.Count > 0 ? Occupants[0] : null;

        public GridCell(Vector3Int gridPos, Vector3 worldCenter)
        {
            GridPos = gridPos;
            WorldCenter = worldCenter;
        }

        public void AddOccupant(Unit unit)
        {
            if (unit == null) return;
            if (!Occupants.Contains(unit))
                Occupants.Add(unit);
        }

        public void RemoveOccupant(Unit unit)
        {
            if (unit == null) return;
            Occupants.Remove(unit);
        }

        public override string ToString() => $"{GridPos} (Taken={IsTaken})";
    }
}
