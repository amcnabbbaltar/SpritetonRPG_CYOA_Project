using UnityEngine;

// Reusable quest step that completes when a pressure plate or group is activated.
// Set targetPlateId to require a single specific plate, or targetGroupId to require
// all plates in a group. If both are set, targetGroupId takes priority.
public class ActivatePressurePlateQuestStep : QuestStep
{
    [Header("Target")]
    [Tooltip("Complete when this specific plate is activated. Leave blank to use group instead.")]
    [SerializeField] private string targetPlateId = "";
    [Tooltip("Complete when all plates in this group are activated. Takes priority over targetPlateId.")]
    [SerializeField] private string targetGroupId = "";

    private void OnEnable()
    {
        GameEventsManager.instance.worldEvents.onPressurePlateActivated += OnPlateActivated;
        GameEventsManager.instance.worldEvents.onPressurePlateGroupSolved += OnGroupSolved;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.worldEvents.onPressurePlateActivated -= OnPlateActivated;
        GameEventsManager.instance.worldEvents.onPressurePlateGroupSolved -= OnGroupSolved;
    }

    private void OnPlateActivated(string plateId, string groupId)
    {
        if (string.IsNullOrEmpty(targetGroupId) && plateId == targetPlateId)
        {
            ChangeState("activated", "Pressure plate activated!");
            FinishQuestStep();
        }
    }

    private void OnGroupSolved(string groupId)
    {
        if (!string.IsNullOrEmpty(targetGroupId) && groupId == targetGroupId)
        {
            ChangeState("solved", "All pressure plates activated!");
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state) { }
}
