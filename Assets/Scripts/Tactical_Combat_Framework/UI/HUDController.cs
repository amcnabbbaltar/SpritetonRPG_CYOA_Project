using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tactics2D
{
    /// <summary>
    /// Displays turn phase, round number, units-acted progress, and enemy count.
    /// Wire up the serialized fields in the Inspector and call the Update methods
    /// from TurnManager at the appropriate moments.
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        public static HUDController Instance { get; private set; }

        [Header("Phase Banner")]
        [SerializeField] private TMP_Text phaseText;
        [SerializeField] private Image phaseBanner;
        [SerializeField] private Color playerPhaseColor = new Color(0.2f, 0.6f, 1f);
        [SerializeField] private Color enemyPhaseColor  = new Color(1f, 0.3f, 0.3f);

        [Header("Round & Progress")]
        [SerializeField] private TMP_Text roundText;
        [SerializeField] private TMP_Text actedText;   // "2 / 3 acted"
        [SerializeField] private TMP_Text enemyText;   // "Enemies: 4"

        [Header("Selected Unit Info")]
        [SerializeField] private GameObject unitInfoPanel;
        [SerializeField] private TMP_Text unitNameText;
        [SerializeField] private TMP_Text unitStatsText;

        [Header("End Turn Button")]
        [SerializeField] private Button endTurnButton;

        private PlayerController playerController;
        private int round = 1;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
            if (endTurnButton != null)
                endTurnButton.onClick.AddListener(() => playerController.ForceEndTurn());

            if (unitInfoPanel != null)
                unitInfoPanel.SetActive(false);
        }

        // ── Called by TurnManager ──────────────────────────────────────────────

        /// <summary>Set the phase banner and enable/disable the End Turn button.</summary>
        public void SetPhase(bool isPlayerTurn)
        {
            if (phaseText != null)
                phaseText.text = isPlayerTurn ? "PLAYER PHASE" : "ENEMY PHASE";

            if (phaseBanner != null)
                phaseBanner.color = isPlayerTurn ? playerPhaseColor : enemyPhaseColor;

            if (endTurnButton != null)
                endTurnButton.interactable = isPlayerTurn;
        }

        /// <summary>Update how many player units have acted this round.</summary>
        public void SetActedProgress(int acted, int total)
        {
            if (actedText != null)
                actedText.text = $"{acted} / {total} acted";
        }

        /// <summary>Update the surviving enemy count.</summary>
        public void SetEnemyCount(int count)
        {
            if (enemyText != null)
                enemyText.text = $"Enemies: {count}";
        }

        /// <summary>Advance and display the round counter.</summary>
        public void NextRound()
        {
            round++;
            RefreshRoundText();
        }

        // ── Called by PlayerController on unit selection ───────────────────────

        /// <summary>Show stats for the currently selected unit, or hide the panel if null.</summary>
        public void SetSelectedUnit(Unit unit)
        {
            if (unitInfoPanel == null) return;

            if (unit == null)
            {
                unitInfoPanel.SetActive(false);
                return;
            }

            unitInfoPanel.SetActive(true);

            if (unitNameText != null)
                unitNameText.text = unit.UnitName;

            if (unitStatsText != null)
                unitStatsText.text =
                    $"HP:  {unit.Stats.currentHP} / {unit.Stats.maxHP}\n" +
                    $"Move: {unit.Stats.maxMove}\n" +
                    $"Atk:  {unit.Stats.attackPower}";
        }

        // ── Private helpers ────────────────────────────────────────────────────

        private void RefreshRoundText()
        {
            if (roundText != null)
                roundText.text = $"Round {round}";
        }
    }
}
