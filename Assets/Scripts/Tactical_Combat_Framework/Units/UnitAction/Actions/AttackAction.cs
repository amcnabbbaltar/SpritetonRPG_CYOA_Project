using System.Collections;
using UnityEngine;

namespace Tactics2D
{
    [CreateAssetMenu(menuName = "Tactics2D/Actions/Attack")]
    public class AttackAction : ScriptableObject, IUnitAction
    {
        public string ActionName => "Attack";

        [Header("Attack Settings")]
        public float attackSpeed = 5f;
        public float attackDelay = 0.15f;
        public float flashDuration = 0.1f;
        public string attackTriggerName = "doAttack";
        [Header("Effects")]
        public GameObject hitEffectPrefab;

        public bool CanExecute(Unit unit)
        {
            return unit != null && unit.IsAlive && !unit.IsBusy;
        }

        public IEnumerator Execute(Unit unit)
        {
            Debug.Log($"[AttackAction] {unit.UnitName} starting attack action...");

            var grid = GridManager.Instance;
            if (grid == null || unit.CurrentCell == null)
            {
                Debug.LogWarning("[AttackAction] Grid or unit cell not found, aborting attack.");
                yield break;
            }

            // Find nearby enemy within range
            Unit target = FindClosestEnemy(unit, grid);
            if (target == null)
            {
                Debug.Log($"[AttackAction] {unit.UnitName} found no valid target in range.");
                yield break;
            }

            Debug.Log($"[AttackAction] {unit.UnitName} attacking {target.UnitName} (HP: {target.Stats.currentHP}/{target.Stats.maxHP})");

            unit.IsBusy = true;

            // Move toward the target briefly (lunge animation)
            Vector3 startPos = unit.transform.position;
            Vector3 targetPos = target.transform.position;
            Vector3 midPoint = (startPos + targetPos) / 2f;
            
             Animator anim = unit.GetComponent<Animator>();
            if (anim != null)
            {
                anim.ResetTrigger(attackTriggerName);
                anim.SetTrigger(attackTriggerName);
                Debug.Log($"[AttackAction] Triggered animation '{attackTriggerName}' on {unit.UnitName}.");
            }
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * attackSpeed;
                unit.transform.position = Vector3.Lerp(startPos, midPoint, Mathf.PingPong(t, 0.5f));
                yield return null;
            }

            Debug.Log($"[AttackAction] {unit.UnitName} reached target position, applying hit effects.");

            // Trigger hit feedback
            if (hitEffectPrefab)
            {
                Object.Instantiate(hitEffectPrefab, target.transform.position, Quaternion.identity);
                Debug.Log("[AttackAction] Hit effect instantiated.");
            }

            

            // Optional: Wait for animation to progress before hit
            yield return new WaitForSeconds(attackDelay);

            yield return new WaitForSeconds(attackDelay);

            // Apply damage and flash target
            int dmg = unit.Stats.attackPower;
            target.TakeDamage(dmg);

            Debug.Log($"[AttackAction] {target.UnitName} took {dmg} damage → HP now {target.Stats.currentHP}/{target.Stats.maxHP}");

            yield return FlashDamage(target);

            // Return to original position
            unit.transform.position = startPos;
            unit.IsBusy = false;

            Debug.Log($"[AttackAction] {unit.UnitName} finished attack action.");
        }

        private Unit FindClosestEnemy(Unit unit, GridManager grid)
        {
            Unit closest = null;
            int bestDist = int.MaxValue;

            foreach (var cell in grid.Cells.Values)
            {
                var occ = cell.Occupant;
                if (occ != null && occ.Team != unit.Team)
                {
                    int dist = Mathf.Abs(cell.GridPos.x - unit.CurrentCell.GridPos.x) +
                               Mathf.Abs(cell.GridPos.y - unit.CurrentCell.GridPos.y);

                    if (dist < bestDist && dist <= unit.Stats.attackRange)
                    {
                        closest = occ;
                        bestDist = dist;
                    }
                }
            }

            if (closest != null)
                Debug.Log($"[AttackAction] Closest enemy found: {closest.UnitName} at distance {bestDist}");
            else
                Debug.Log("[AttackAction] No enemy found within attack range.");

            return closest;
        }

        private IEnumerator FlashDamage(Unit target)
        {
            if (target == null)
                yield break;

            var sr = target.Sprite;

            // Ensure the SpriteRenderer reference is valid
            if (sr == null)
            {
                Debug.LogWarning("[AttackAction] SpriteRenderer missing or destroyed during FlashDamage.");
                yield break;
            }

            // Avoid flash if unit died and got destroyed
            if (target == null || target.gameObject == null)
                yield break;

            Color original = sr.color;
            sr.color = target.Stats.damageColor;

            Debug.Log($"[AttackAction] Flashing {target.UnitName} in {target.Stats.damageColor} for {flashDuration}s.");

            float elapsed = 0f;
            while (elapsed < flashDuration)
            {
                if (sr == null) yield break; // renderer destroyed mid-loop
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (sr != null)
                sr.color = original;
        }

    }
}
