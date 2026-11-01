using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HillZone : NetworkBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    
    // A HashSet to keep track of which players are currently in the objective area
    HashSet<Collider> redPlayersInZone = new HashSet<Collider>();
    HashSet<Collider> bluePlayersInZone = new HashSet<Collider>();

    private float scoreTimer = 0f;

    void Update()
    {
        if (!IsServer) return;
        scoreTimer += Time.deltaTime;

        if (scoreTimer >= 1f)
        {
            scoreTimer = 0f;
            AreaCheck();
        }
    }

    // When the players enter the objective area
    void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (other.gameObject.CompareTag("TeamRed"))
        {
            Debug.Log("Team Red is in the hill zone");
            redPlayersInZone.Add(other);
        }
        else if (other.gameObject.CompareTag("TeamBlue"))
        {
            Debug.Log("Team Blue is in the hill zone");
            bluePlayersInZone.Add(other);
        }

        AreaCheck();
    } 

    // When the players exit the objective area
    void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;
        if (other.gameObject.CompareTag("TeamRed"))
        {
            Debug.Log("Team Red has exited the hill zone");
            redPlayersInZone.Remove(other);
        }
        else if (other.gameObject.CompareTag("TeamBlue"))
        {
            Debug.Log("Team Blue has exited the hill zone");
            bluePlayersInZone.Remove(other);
        }

        AreaCheck();
    }

    // Check if both teams are in the objective area
    void AreaCheck()
    {
        bool redTeam = redPlayersInZone.Count > 0;
        bool blueTeam = bluePlayersInZone.Count > 0;
        
        if (redTeam && blueTeam)
        {
            Debug.Log("Both teams are contesting the hill!");
        }
        else if (redTeam)
        {
            Debug.Log("Team Red is controlling the hill!");
            scoreManager.RedPointIncrease();
        }
        else if (blueTeam)
        {
            Debug.Log("Team Blue is controlling the hill!");
            scoreManager.BluePointIncrease();
        }
    }
}


/*using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Collect : MonoBehaviour
{
    private GameObject powerUp;
    private bool canPickUp;

    public PlayerMovement playerMovement;

    public float speedMultiplier = 2f;
    public float boostDuration = 7f;

    void Update()
    {
        if (canPickUp &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            PickUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            powerUp = other.gameObject;
            canPickUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == powerUp)
        {
            powerUp = null;
            canPickUp = false;
        }
    }

    private void PickUp()
    {
        if (powerUp == null) return;

        Debug.Log("Speed boost collected!");

        playerMovement.StartSpeedBoost(
            speedMultiplier, boostDuration
        );

        Destroy(powerUp);
        powerUp = null;
        canPickUp = false;
    }
}*/