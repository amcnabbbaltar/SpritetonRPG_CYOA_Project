using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyDetector : MonoBehaviour
{

    public float detectionRadius = 1.5f; // How far you want to look for allies
    public static int allyCount;   // The amount of allies found
    public LayerMask allyLayer;
    public static int cohesionRate;

    private void Start()
    {
        FindAllies();
    }

    public void FindAllies() // Make sure the allies have some sort of 2D collider for this to work
    {
        int allyCount = 0;
        // Detecting all that collides in the circumference around the player, checking allies in the ally layer
        Collider2D[] allies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, allyLayer);

        foreach (Collider2D ally in allies)
        {
            allyCount++;
        }

        cohesionRate = allyCount * 25;

        Debug.Log("Ally Count: " + allyCount);
        Debug.Log("Cohesion Rate: " + cohesionRate);

    }

    //void FindAllies()
    //{
    //    // Reset before recounting
    //    allyCount = 0;

    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

    //    foreach (GameObject player in players)
    //    {
    //        // Don’t count self (if this script is on a player), because it's not an ally if it's yourself
    //        if (player != gameObject)
    //        {
    //            allyCount++;
    //        }
    //    }
    //}

    // Just drawing the circumference so it can be visual in the Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

   
}
