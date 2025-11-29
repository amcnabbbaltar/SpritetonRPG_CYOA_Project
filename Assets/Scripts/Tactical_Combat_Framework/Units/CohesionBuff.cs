using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Tactics2D;

public class CohesionBuff : MonoBehaviour
{
     private Unit unit;
    private AllyDetector allyDetector; // Script made by Marc
    private int baseAttack; // to store base attack value and be modified
    
    [SerializeField] private int cohesionRateThreshold = 25; //threshold of allies nearby to get buff (each ally is 25)
    [SerializeField] private int overallCohesionThreshold = 5; //threshold of Morale to get buff (cohesion level from normal scene)
    [SerializeField] private int teamBonus = 2; //bonus damage when close to allies
    [SerializeField] private int flatBonus = 2; // Flat bonus if overall morale is good

    private bool teamBuffed;
    private bool overallCohesionBuffed;
    private int initialBaseDamage;
    private int modifiedBaseDamage;
    private void Awake()
    {
        unit = GetComponent<Unit>();
        allyDetector = GetComponent<AllyDetector>();
        initialBaseDamage = unit.Stats.attackPower; // Store the initial base damage
        modifiedBaseDamage = unit.Stats.attackPower; // Always keep updated for modified Damage

        if (unit != null)
           {
             baseAttack = unit.Stats.attackPower; // Remember its base attack value at the start
           }


        if (ConstantValue.instance != null) // Access the singleton persistent instance of ConstantValue to keep overall cohesion level
            //(baseAttack will hold modified value if overall buff is active)
            {
                if (ConstantValue.instance.currentPlayerCohesionLevel >= overallCohesionThreshold)
                {
                    overallCohesionBuffed = true;
                    baseAttack += flatBonus; // apply flat bonus 
                    modifiedBaseDamage = unit.Stats.attackPower += flatBonus; // Always keep updated for modified Damage
                    Debug.Log("Unit has overall Cohesion Buff! +2 DMG!");
                }
            }
            else
            {
                overallCohesionBuffed = false;
                Debug.LogError("ConstantValue instance not found in scene.");
            }

    }
    

    private void Update()
    {
        if (allyDetector == null || unit == null) return;

        int allyCohesionRate = (int)AllyDetector.cohesionRate; // must be public or have a getter
        if (teamBuffed != true && allyCohesionRate >= cohesionRateThreshold)
        {
            unit.Stats.attackPower = baseAttack + teamBonus; // apply buff to the runtime clone
            teamBuffed = true;
            Debug.Log("Unit is now team buffed! +2 DMG!");
     
      
        }
        else if (teamBuffed == true && allyCohesionRate < cohesionRateThreshold)
        {
            unit.Stats.attackPower = baseAttack; // revert
            teamBuffed = false;
            Debug.Log("Unit has lost team buffed! -2 DMG!");

        }

        modifiedBaseDamage = unit.Stats.attackPower; // Always keep updated for modified Damage

    }

    // public void getUnitStats(){
    //     Debug.Log("Initial Base Attack Power: " + initialBaseDamage);
    //     Debug.Log("team buffed? (+2 DMG)?: " + teamBuffed);
    //     Debug.Log("Overall Morale buffed? (+2 DMG)?: " + overallCohesionBuffed);
    //     Debug.Log("Modified Base Attack Power: " + modifiedBaseDamage);
    // }
}