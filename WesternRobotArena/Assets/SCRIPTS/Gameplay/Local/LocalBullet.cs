
using UnityEngine;

public class LocalBullet : MonoBehaviour
{
    [SerializeField] private EnemyHealthLocal enemyHealth;

    [SerializeField]
    private float speed = 15f;

    [SerializeField]
    private float lifeTime = 3f;

    [SerializeField]
    public float bulletDamage = 20f;

    private GameObject owner;
    private float lifeTimer;

    public void SetOwner(GameObject player)
    {
        owner = player;
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null)
            return;

        var playerHealth = other.GetComponentInParent<PlayerHealthLocal>();

        var enemyHealth = other.GetComponentInParent<EnemyHealthLocal>();

        // Stops the player damaging themselves
        if (playerHealth != null && playerHealth.gameObject == owner)
            return;

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(bulletDamage);
        }
           
        if(enemyHealth != null)
        {
            enemyHealth.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
