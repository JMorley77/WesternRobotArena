using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HillZone : NetworkBehaviour
{
    [SerializeField] private ScoreManager scoreManager;

    [Header("Zone Colours")]
    public GameObject blue;
    public GameObject red;
    public GameObject yellow;
    public GameObject white;

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


    void AreaCheck()
    {
        bool redTeam = redPlayersInZone.Count > 0;
        bool blueTeam = bluePlayersInZone.Count > 0;
        
        if (redTeam && blueTeam)
        {
            Debug.Log("Both teams are contesting the hill!");
            yellow.SetActive(true);
            red.SetActive(false);
            blue.SetActive(false);
            white.SetActive(false);

        }
        else if (redTeam)
        {
            Debug.Log("Team Red is controlling the hill!");
            scoreManager.RedPointIncrease();
            yellow.SetActive(false);
            red.SetActive(true);
            blue.SetActive(false);
            white.SetActive(false);
        }
        else if (blueTeam)
        {
            Debug.Log("Team Blue is controlling the hill!");
            scoreManager.BluePointIncrease();
            yellow.SetActive(false);
            red.SetActive(false);
            blue.SetActive(true);
            white.SetActive(false);
        }
        else if (!blueTeam && !redTeam)
        {
            yellow.SetActive(false);
            red.SetActive(false);
            blue.SetActive(false);
            white.SetActive(true);
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