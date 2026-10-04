using System.Collections.Generic;
using UnityEngine;

public class HillZone : MonoBehaviour
{
    // A HashSet to keep track of which players are currently in the objective area
    HashSet<string> playersInZone = new HashSet<string>();

    // When the players enter the objective area
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TeamRed"))
        {
            Debug.Log("Team Red is in the hill zone");
            playersInZone.Add("TeamRed");
        }
        else if (other.gameObject.CompareTag("TeamBlue"))
        {
            Debug.Log("Team Blue is in the hill zone");
            playersInZone.Add("TeamBlue");
        }

        AreaCheck();
    } 

    // When the players exit the objective area
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("TeamRed"))
        {
            Debug.Log("Team Red has exited the hill zone");
            playersInZone.Remove("TeamRed");
        }
        else if (other.gameObject.CompareTag("TeamBlue"))
        {
            Debug.Log("Team Blue has exited the hill zone");
            playersInZone.Remove("TeamBlue");
        }

        AreaCheck();
    }

    // Check if both teams are in the objective area
    void AreaCheck()
    {
        if (playersInZone.Contains("TeamRed") && playersInZone.Contains("TeamBlue"))
        {
            Debug.Log("Both teams are contesting the hill!");
        }
    }
}
