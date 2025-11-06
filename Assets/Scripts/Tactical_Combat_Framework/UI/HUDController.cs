/*using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tactics2D
{
    /// <summary>
    /// Displays player turn info, selected unit stats, and tile information under the cursor.
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text turnText;
        [SerializeField] private TMP_Text unitInfoText;
        [SerializeField] private TMP_Text tileInfoText;
        [SerializeField] private Button endTurnButton;

        private PlayerController playerController;

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
            if (endTurnButton != null)
                endTurnButton.onClick.AddListener(() => playerController.ForceEndTurn());
        }

        #region Turn & Unit Info
        public void UpdateTurn(bool isPlayerTurn)
        {
            if (turnText != null)
                turnText.text = isPlayerTurn ? "Player Turn" : "Enemy Turn";
        }

        public void UpdateUnitInfo(Unit unit)
        {
            if (unitInfoText == null) return;

            if (unit == null)
                unitInfoText.text = "";
            else
                unitInfoText.text =
                    $"{unit.UnitName}\n" +
                    $"HP: {unit.CurrentHP}/{unit.MaxHP}\n" +
                    $"Move: {unit.MaxMove}\n" +
                    $"Atk: {unit.AttackPower}";
        }
        #endregion

    
    }
}*/
