using UnityEngine;

public class WorldDoor : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] private string groupId;
    [Tooltip("How many plates in the group must be active to open this door.")]
    [SerializeField] private int requiredActivations = 1;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Animator animator;
    [SerializeField] private string openTrigger = "Open";
    [SerializeField] private string closeTrigger = "Close";

    [Header("Physics")]
    [Tooltip("Collider to disable when open so the player can pass through.")]
    [SerializeField] private Collider2D blockingCollider;

    private int activeCount = 0;
    private bool isOpen = false;

    private void OnEnable()
    {
        GameEventsManager.instance.worldEvents.onPressurePlateActivated += OnPlateActivated;
        GameEventsManager.instance.worldEvents.onPressurePlateDeactivated += OnPlateDeactivated;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.worldEvents.onPressurePlateActivated -= OnPlateActivated;
        GameEventsManager.instance.worldEvents.onPressurePlateDeactivated -= OnPlateDeactivated;
    }

    private void OnPlateActivated(string plateId, string plateGroupId)
    {
        if (plateGroupId != groupId) return;

        activeCount++;
        if (activeCount >= requiredActivations && !isOpen)
        {
            Open();
            GameEventsManager.instance.worldEvents.PressurePlateGroupSolved(groupId);
        }
    }

    private void OnPlateDeactivated(string plateId, string plateGroupId)
    {
        if (plateGroupId != groupId) return;

        activeCount = Mathf.Max(0, activeCount - 1);
        if (activeCount < requiredActivations && isOpen)
        {
            Close();
            GameEventsManager.instance.worldEvents.PressurePlateGroupReset(groupId);
        }
    }

    private void Open()
    {
        isOpen = true;
        if (blockingCollider != null) blockingCollider.enabled = false;
        if (spriteRenderer != null && openSprite != null) spriteRenderer.sprite = openSprite;
        if (animator != null && !string.IsNullOrEmpty(openTrigger)) animator.SetTrigger(openTrigger);
    }

    private void Close()
    {
        isOpen = false;
        if (blockingCollider != null) blockingCollider.enabled = true;
        if (spriteRenderer != null && closedSprite != null) spriteRenderer.sprite = closedSprite;
        if (animator != null && !string.IsNullOrEmpty(closeTrigger)) animator.SetTrigger(closeTrigger);
    }
}
