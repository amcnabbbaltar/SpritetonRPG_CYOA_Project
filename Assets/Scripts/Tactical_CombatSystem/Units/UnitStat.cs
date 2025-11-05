using UnityEngine;

namespace Tactics2D
{
    [CreateAssetMenu(menuName = "Tactics2D/Unit Stats", fileName = "NewUnitStats")]
    public class UnitStats : ScriptableObject
    {
        [Header("Identity")]
        public string unitName = "Unit";
        public Team team = Team.Player;

        [Header("Attributes")]
        public int maxHP = 10;
        [HideInInspector] public int currentHP; // Runtime value
        public int attackPower = 4;
        public int attackRange = 1;
        public int maxMove = 5;

        [Header("Movement")]
        public float moveSpeed = 8f;

        [Header("Visuals")]
        public Color damageColor = Color.red;
    }
}
