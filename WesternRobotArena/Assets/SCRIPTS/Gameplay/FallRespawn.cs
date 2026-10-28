using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Transform outlawSpawnPoint;
    [SerializeField] private Transform sheriffSpawnPoint;


    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("entered");
        playerHealth.Die();
        playerHealth.TakeDamage(10f);
    }
}
