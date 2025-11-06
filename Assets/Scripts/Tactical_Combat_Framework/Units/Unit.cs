using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics2D
{
    public class Unit : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private UnitStats stats;
        [SerializeField] private List<ScriptableObject> actionAssets;

        private readonly List<IUnitAction> actions = new();

        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Animator animator;

        public UnitStats Stats => stats;
        public string UnitName => stats.unitName;
        public Team Team => stats.team;
        public bool IsAlive => stats.currentHP > 0;
        public bool IsBusy { get; set; }
        public GridCell CurrentCell { get; private set; }
        public SpriteRenderer Sprite => spriteRenderer;

        

        private bool teleporting;
        public static System.Action<Unit> OnTeleported;

        private void Awake()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            // Clone the stats asset per unit
            stats = Instantiate(stats);
            stats.currentHP = stats.maxHP;

            // Instantiate each action asset so they behave independently
            actions.Clear();
            foreach (var so in actionAssets)
            {
                if (so is IUnitAction action)
                {
                    var clone = Instantiate(so) as ScriptableObject;
                    if (clone is IUnitAction runtimeAction)
                    {
                        actions.Add(runtimeAction);
                        Debug.Log($"[Unit] Added runtime action: {clone.name}");
                    }
                }
                else
                {
                    Debug.LogWarning($"[Unit] {so.name} does not implement IUnitAction!");
                }
            }
        }

        private void Start()
        {
            var grid = GridManager.Instance;
            animator = GetComponent<Animator>();
            if (grid != null)
            {
                var cell = grid.CellFromWorld(transform.position);
                if (cell != null)
                    MoveIntoCell(cell, instant: true);
            }
        }

        #region Movement
        public void MoveIntoCell(GridCell newCell, bool instant = false)
        {
            if (newCell == null) return;

            if (CurrentCell != null)
            {
                CurrentCell.RemoveOccupant(this);
                GridManager.Instance.OnUnitExitCell(this, CurrentCell);
            }

            CurrentCell = newCell;
            CurrentCell.AddOccupant(this);
            GridManager.Instance.OnUnitEnterCell(this, CurrentCell);

            if (instant)
                transform.position = newCell.WorldCenter;
        }

        public IEnumerator MoveAlong(List<GridCell> path)
        {
            if (path == null || path.Count == 0) yield break;
            if (IsBusy) yield break;

            IsBusy = true;

            // --- ANIM START ---
            if (animator)
                animator.SetBool("isWalking", true);

            for (int i = 1; i < path.Count; i++)
            {
                GridCell target = path[i];
                Vector3 start = transform.position;
                Vector3 end = target.WorldCenter;

                // --- FACE DIRECTION ---
                if (spriteRenderer != null)
                {
                    float dirX = end.x - start.x;

                    // Flip horizontally if moving left, face right otherwise
                    if (Mathf.Abs(dirX) > 0.05f)
                        spriteRenderer.flipX = dirX < 0;
                }

                float t = 0f;
                while (t < 1f)
                {
                    t += Time.deltaTime * stats.moveSpeed;
                    transform.position = Vector3.Lerp(start, end, t);
                    yield return null;
                }

                MoveIntoCell(target, instant: true);
            }

            // --- ANIM END ---
            if (animator)
                animator.SetBool("isWalking", false);

            IsBusy = false;
        }

        public void SetCurrentCell(GridCell newCell, bool updatePosition = true)
        {
            if (newCell == null) return;

            if (CurrentCell != null)
                CurrentCell.RemoveOccupant(this);

            CurrentCell = newCell;
            newCell.AddOccupant(this);

            if (updatePosition)
                transform.position = newCell.WorldCenter;

            GridManager.Instance?.OnUnitEnterCell(this, newCell);
        }
        #endregion

        #region Combat
        public void TakeDamage(int dmg)
        {
            if (!IsAlive) return;

            stats.currentHP -= dmg;
            if (stats.currentHP < 0)
                stats.currentHP = 0;

            Debug.Log($"[Unit] {UnitName} took {dmg} damage ({stats.currentHP}/{stats.maxHP})");

            if (stats.currentHP <= 0)
                Die();
        }

        private void Die()
        {
            Debug.Log($"[Unit] {UnitName} has died.");
            if (CurrentCell != null)
                CurrentCell.RemoveOccupant(this);
            Destroy(gameObject);
        }
        #endregion

        #region Actions
        public IEnumerable<IUnitAction> GetActions() => actions;

        public void ExecuteAction(IUnitAction action)
        {
            if (action == null)
            {
                Debug.LogWarning($"[Unit] Tried to execute a null action on {UnitName}");
                return;
            }

            if (!action.CanExecute(this))
            {
                Debug.Log($"[Unit] Cannot execute action {action.GetType().Name} — busy or invalid.");
                return;
            }

            Debug.Log($"[Unit] {UnitName} executing action: {action.GetType().Name}");
            StartCoroutine(action.Execute(this));
        }

        public void ExecuteAction<T>() where T : IUnitAction
        {
            foreach (var act in actions)
            {
                if (act is T)
                {
                    ExecuteAction(act);
                    return;
                }
            }
            Debug.LogWarning($"[Unit] {UnitName} has no action of type {typeof(T).Name}");
        }
        #endregion
    }
}
