using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tactics2D
{
    [CreateAssetMenu(menuName = "Tactics2D/Behaviors/Scene Transition Behavior")]
    public class SceneTransitionBehaviour : ScriptableObject, ITileBehavior
    {
        [Header("Scene Transition Settings")]
        [Tooltip("Name of the scene to load when a unit steps on this tile.")]
        [SerializeField] public string sceneName;

        [Tooltip("If true, only player units trigger the transition. Enemies are ignored.")]
        [SerializeField] public bool playerOnly = true;

        [Tooltip("Optional VFX spawned at the unit's position before transitioning.")]
        [SerializeField] public GameObject transitionEffect;

        public void Initialize(GridManager grid, Vector3Int position) { }

        public void OnUnitEnter(Unit unit, GridManager grid)
        {
            if (playerOnly && unit.Team != Team.Player) return;

            if (transitionEffect != null)
                Instantiate(transitionEffect, unit.transform.position, Quaternion.identity);

            Debug.Log($"[SceneTransition] Loading scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }

        public void OnUnitExit(Unit unit, GridManager grid) { }
    }
}
