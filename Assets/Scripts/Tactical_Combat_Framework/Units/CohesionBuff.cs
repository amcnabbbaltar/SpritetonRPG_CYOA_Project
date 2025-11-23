using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Tactics2D;

public class CohesionBuff : MonoBehaviour
{
     private Unit unit;
    private AllyDetector allyDetector; // example script that has CohesionRate
    private int baseAttack;

    [SerializeField] private float threshold = 25f;
    [SerializeField] private int bonus = 2;
    private bool buffed;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        allyDetector = GetComponent<AllyDetector>();

        if (unit != null)
            baseAttack = unit.Stats.attackPower; // Unit clones UnitStats in Awake, so this is safe
    }

    private void Update()
    {
        if (allyDetector == null || unit == null) return;

        float rate = (float)AllyDetector.cohesionRate; // must be public or have a getter
        if (!buffed && rate >= threshold)
        {
            unit.Stats.attackPower = baseAttack + bonus; // apply buff to the runtime clone
            buffed = true;           
            Debug.Log("User is buffed!");

        }
        else if (buffed && rate < threshold)
        {
            unit.Stats.attackPower = baseAttack; // revert
            buffed = false;
            Debug.Log("User is debuffed!");

        }
    }
}