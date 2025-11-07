using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class CLevelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private TextMeshProUGUI CLevelUIText;
    [SerializeField] private Image CLevelUIBar;

    private float currentPlayerCohesionLevel = 0;
    private float maxPlayerCohesionLevel = 100;



    private void OnEnable() 
    {
        GameEventsManager.instance.playerEvents.onPlayerCohesionLevelChange += PlayerCohesionLevelChange;
        GameEventsManager.instance.inputEvents.onQuestLogTogglePressed += QuestLogTogglePressed;

    }

    private void OnDisable() 
    {
        GameEventsManager.instance.playerEvents.onPlayerCohesionLevelChange -= PlayerCohesionLevelChange;
        GameEventsManager.instance.inputEvents.onQuestLogTogglePressed -= QuestLogTogglePressed;

    }

    private void QuestLogTogglePressed()
    {
        if (contentParent.activeInHierarchy)
        {
            HideUI();
        }
        else
        {
            ShowUI();
        }
    }

    private void ShowUI()
    {
        contentParent.SetActive(true);
        //this is already used in the QuestLog
        //GameEventsManager.instance.playerEvents.DisablePlayerMovement();
        
    }

    private void HideUI()
    {
        contentParent.SetActive(false);
        //GameEventsManager.instance.playerEvents.EnablePlayerMovement();
        //EventSystem.current.SetSelectedGameObject(null);
    }



    private void PlayerCohesionLevelChange(int cohesionlevel) 
    {
        CLevelUIText.text = cohesionlevel.ToString();
        currentPlayerCohesionLevel = cohesionlevel;
        CLevelUIBar.fillAmount = currentPlayerCohesionLevel / maxPlayerCohesionLevel;
    }
}
