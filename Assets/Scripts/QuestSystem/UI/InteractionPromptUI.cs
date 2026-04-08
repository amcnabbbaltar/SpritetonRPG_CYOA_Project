using UnityEngine;
using TMPro;

/// <summary>
/// Attach to a world-space Canvas child of a QuestPoint to display
/// an interaction hint (e.g. "Press [Enter] to talk") above an NPC.
/// QuestPoint will call Show() / Hide() based on player proximity.
/// </summary>
public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private string promptMessage = "Press [Enter] to talk";

    private void Awake()
    {
        if (promptText != null)
            promptText.text = promptMessage;

        gameObject.SetActive(false);
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}
