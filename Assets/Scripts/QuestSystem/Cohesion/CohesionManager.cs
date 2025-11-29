using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CohesionManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int startingCohesionLevel = 3;

    private static CohesionManager instance;

    public int currentPlayerCohesionLevel { get; private set; }

    private void Awake()
    {
        currentPlayerCohesionLevel = startingCohesionLevel; //give it a starting level
    }

    private void OnEnable() 
    {
        GameEventsManager.instance.playerEvents.onPlayerCohesionLevelGained += PlayerCohesionLevelGained;
       // GameEventsManager.instance.playerEvents.onPlayerLevelChange += onPlayerLevelChange;

    }

    private void OnDisable() 
    {
        GameEventsManager.instance.playerEvents.onPlayerCohesionLevelGained -= PlayerCohesionLevelGained;
       // GameEventsManager.instance.playerEvents.onPlayerLevelChange -= onPlayerLevelChange;

    }

    // this is called for subscribers (Dialogue) to have the current amount 
    private void Start()
    {
    GameEventsManager.instance.playerEvents.PlayerCohesionLevelChange(currentPlayerCohesionLevel);
    }

    private void PlayerCohesionLevelGained(int Cohesionlevel)
    {
        currentPlayerCohesionLevel += Cohesionlevel;
        GameEventsManager.instance.playerEvents.PlayerCohesionLevelChange(currentPlayerCohesionLevel);

    }
/*
if ever we make a cohesion setup like start or something like that*
    private void PlayerCohesionLevelChange(int Cohesionlevel)
    {
        currentPlayerCohesionLevel = Cohesionlevel;

    }
    */
    
    
}
