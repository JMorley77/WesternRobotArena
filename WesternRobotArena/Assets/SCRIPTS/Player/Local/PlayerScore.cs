using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private bool isRed;
    [SerializeField] private ScoreManagerLocal scoreManager;

    private PlayerHealthLocal health;

    private void Awake()
    {
        health = GetComponent<PlayerHealthLocal>();
    }

    private void OnEnable()
    {
        health.OnPlayerDied += AddScore;
    }

    private void OnDisable()
    {
        health.OnPlayerDied -= AddScore;
    }

    private void AddScore(PlayerHealthLocal h)
    {
        if (isRed)
            scoreManager.BluePointIncrease(1);   
        else
            scoreManager.RedPointIncrease(1);    
    }
}
