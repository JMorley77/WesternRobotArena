using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager Instance;

    [SerializeField] private GameObject loadingPanel;

    private NetworkManager networkManager;
    private bool isLoading;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        loadingPanel.SetActive(false);
    }

    private void Update()
    {
        if (networkManager == null)
        {
            NetworkManager nm = NetworkManager.Singleton;

            if (nm != null && nm.IsListening)
            {
                networkManager = nm;

                networkManager.SceneManager.OnSceneEvent+= OnSceneEvent;
            }
        }
    }

    private void OnSceneEvent(SceneEvent sceneEvent)
    {
        // Scene loading begins
        if (sceneEvent.SceneEventType == SceneEventType.Load)
        {
            isLoading = true;
            ShowLoading();
        }

        // All participating players finished loading
        if (sceneEvent.SceneEventType ==SceneEventType.LoadEventCompleted)
        {
            if (isLoading)
            {
                isLoading = false;
                HideLoading();
            }
        }
    }

    public void ShowLoading()
    {
        loadingPanel.SetActive(true);
    }

    public void HideLoading()
    {
        loadingPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (networkManager != null && networkManager.SceneManager != null)
        {
            networkManager.SceneManager.OnSceneEvent-= OnSceneEvent;
        }

        if (Instance == this)
            Instance = null;
    }
}