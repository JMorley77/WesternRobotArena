using UnityEngine;

public class Sandstorm : MonoBehaviour
{
    [SerializeField] private GameObject tornado;
    [SerializeField] private GameObject dustParticle;

    [SerializeField] private float eventTime;

    public void Awake()
    {
        eventTime = Time.deltaTime;
    }
    public void Update()
    {
        eventTime++;
        if(eventTime >= 6000f)
        {
            tornado.SetActive(true);
            dustParticle.SetActive(true);

        }

        if (eventTime >= 10000f)
        {
            tornado.SetActive(false);
            dustParticle.SetActive(false);

        }
    }


}
