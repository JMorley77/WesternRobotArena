using UnityEngine;

public class Sandstorm : MonoBehaviour
{
    [SerializeField] private GameObject tornado;
    [SerializeField] private GameObject dustParticle;

    [SerializeField] private float eventTime;

    public void Awake()
    {
        eventTime = 0f;
    }
    public void FixedUpdate()
    {
        eventTime+=Time.deltaTime;
        if (eventTime >= 120f)
        {
            tornado.SetActive(true);
            dustParticle.SetActive(true);

        }

        if (eventTime >= 140f)
        {
            tornado.SetActive(false);
            dustParticle.SetActive(false);

        }
    }


}
