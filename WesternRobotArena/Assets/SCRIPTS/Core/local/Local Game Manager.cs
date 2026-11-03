using UnityEngine;
using UnityEngine.InputSystem;

public class LocalGameManager : MonoBehaviour
{
    public enum GameState
    {
        WaitingForPlayers,
        Starting,
        InGame,
        Finished
    }

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 480f;
    [SerializeField] private float startCountdownDuration = 5f;
    [SerializeField] private int requiredPlayers = 2;

    [Header("Player Settings")]
    [SerializeField] private GameObject SheriffPrefab;
    [SerializeField] private GameObject CowboyPrefab;

    [SerializeField] private Transform[] spawnPoints;

    [Header("Camera Settings")]
    [SerializeField] private Camera mainCamera;

    [Header("Game State")]
    [SerializeField]
    private GameState state = GameState.WaitingForPlayers;

    private float timeRemaining;
    private float startTimeRemaining;

    private GameObject[] spawnedPlayers;

    private ArenaCamera arenaCamera;

    public GameState State => state;
    public float TimeRemaining => timeRemaining;
    public float StartTimeRemaining => startTimeRemaining;

    private void Start()
    {
        spawnedPlayers = new GameObject[requiredPlayers];

        timeRemaining = gameDuration;
        startTimeRemaining = startCountdownDuration;

        arenaCamera = mainCamera.GetComponent<ArenaCamera>();

        SpawnLocalPlayers();
    }

    private void SpawnLocalPlayers()
    {
        if (requiredPlayers != 2)
        {
            Debug.LogError("This setup requires exactly two players.");
            return;
        }

        if (spawnPoints == null ||
            spawnPoints.Length < requiredPlayers)
        {
            Debug.LogError("Not enough spawn points assigned!");
            return;
        }

        if (Keyboard.current == null ||
            Mouse.current == null ||
            Gamepad.current == null)
        {
            Debug.LogError(
                "Keyboard, mouse and gamepad are required!"
            );
            return;
        }

        for (int i = 0; i < requiredPlayers; i++)
        {

            GameObject selectedPrefab = i == 0 ? SheriffPrefab : CowboyPrefab;

            if (selectedPrefab == null)
            {
                Debug.LogError($"Player {i + 1} prefab missing!");
                return;
            }

            if (selectedPrefab.GetComponent<PlayerInput>() == null)
            {
                Debug.LogError("Prefab needs a PlayerInput component!");
                return;
            }

            PlayerInput playerInput;

            if (i == 0)
            {
                playerInput = PlayerInput.Instantiate(selectedPrefab,playerIndex: i,controlScheme: "Keyboard&Mouse",pairWithDevices: new InputDevice[]{Keyboard.current, Mouse.current});
            }
            else
            {
                playerInput = PlayerInput.Instantiate(selectedPrefab,playerIndex: i,controlScheme: "Gamepad",pairWithDevice: Gamepad.current);
            }

            if (playerInput == null)
            {
                Debug.LogError($"Could not create Player {i + 1}");
                return;
            }

            GameObject player = playerInput.gameObject;
            Transform spawnPoint = spawnPoints[i];

            player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            spawnedPlayers[i] = player;

            player.tag = i == 0 ? "TeamRed" : "TeamBlue";

            arenaCamera.AddPlayer(player.transform); //tells camera which players have spawned

            Debug.Log( $"Local Player {i + 1} spawned " + $"using {playerInput.currentControlScheme}"
            );
        }

        state = GameState.Starting;
        startTimeRemaining = startCountdownDuration;
    }

    private void Update()
    {
        if (state == GameState.Starting)
        {
            startTimeRemaining -= Time.deltaTime;

            if (startTimeRemaining <= 0f)
            {
                startTimeRemaining = 0f;
                state = GameState.InGame;

                Debug.Log("Local match started!");
            }
        }
        else if (state == GameState.InGame)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                EndMatch();
            }
        }
    }


    public void EndMatch()
    {
        if (state == GameState.Finished)
            return;

        state = GameState.Finished;

        Debug.Log("Local match ended!");
    }

    public GameObject GetPlayer(int playerIndex)
    {
        if (spawnedPlayers == null ||playerIndex < 0 ||playerIndex >= spawnedPlayers.Length)
        {
            return null;
        }

        return spawnedPlayers[playerIndex];
    }
}