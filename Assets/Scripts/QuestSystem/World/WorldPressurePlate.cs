using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WorldPressurePlate : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] private string plateId;
    [SerializeField] private string groupId;

    [Header("Behaviour")]
    [Tooltip("If true, deactivates when the player leaves. If false, stays activated permanently.")]
    [SerializeField] private bool resetOnExit = false;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Animator animator;
    [SerializeField] private string activateTrigger = "Activate";
    [SerializeField] private string deactivateTrigger = "Deactivate";

    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerMovementController>(out _)) return;
        if (isActivated && !resetOnExit) return;

        Activate();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!resetOnExit) return;
        if (!other.TryGetComponent<PlayerMovementController>(out _)) return;

        Deactivate();
    }

    private void Activate()
    {
        isActivated = true;
        UpdateVisuals(true);
        GameEventsManager.instance.worldEvents.PressurePlateActivated(plateId, groupId);
    }

    private void Deactivate()
    {
        isActivated = false;
        UpdateVisuals(false);
        GameEventsManager.instance.worldEvents.PressurePlateDeactivated(plateId, groupId);
    }

    private void UpdateVisuals(bool active)
    {
        if (spriteRenderer != null)
        {
            Sprite next = active ? activeSprite : inactiveSprite;
            if (next != null) spriteRenderer.sprite = next;
        }
        if (animator != null)
        {
            string trigger = active ? activateTrigger : deactivateTrigger;
            if (!string.IsNullOrEmpty(trigger)) animator.SetTrigger(trigger);
        }
    }
}
