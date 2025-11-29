using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tactics2D; //this script will stay in both normal and combat events


public class ConstantValue : MonoBehaviour
{
    [Header("Configuration")]

    public static ConstantValue instance;

    public int currentPlayerCohesionLevel{ get; private set; }

    private void Awake()
    {
        currentPlayerCohesionLevel = 0; //default value
        // if instance already exists and it's not this one, destroy the duplicate
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (transform.parent != null)
        {
        transform.SetParent(null, true);
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // persist root GameObject across scene loads
    
    }

    private void OnEnable() 
    {
        if (GameEventsManager.instance != null){
        GameEventsManager.instance.playerEvents.onPlayerCohesionLevelChange += PlayerCohesionLevelChange;
        }
    }

    private void OnDisable() 
    {
        if (GameEventsManager.instance != null){
        GameEventsManager.instance.playerEvents.onPlayerCohesionLevelChange -= PlayerCohesionLevelChange;
        }
    }
    //used for testing
    private void Update(){
        {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SceneManager.LoadScene("TRPG_Exemple");
        }
    }
    }


    private void PlayerCohesionLevelChange(int Cohesionlevel)
    {
        currentPlayerCohesionLevel = Cohesionlevel;
    }
    
}
