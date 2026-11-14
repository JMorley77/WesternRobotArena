using UnityEngine;


public class SlashEffect : MonoBehaviour
{
    [SerializeField] private TrailRenderer slashTrail;
    [SerializeField] private float slashDuration = 0.2f;


    private float slashTimer;
    private bool isSlashing;

    private void Awake()
    {
        slashTrail.emitting = false;
    }

    private void Update()
    {
        if (!isSlashing)
            return;

        slashTimer -= Time.deltaTime;

        if (slashTimer <= 0f)
        {
            slashTrail.emitting = false;
            isSlashing = false;
        }
    }

    public void PlaySlash()
    {
        slashTrail.Clear();
        slashTrail.emitting = true;

        slashTimer = slashDuration;
        isSlashing = true;
    }
}
