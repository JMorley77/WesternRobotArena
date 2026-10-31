using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField]
    private float speed = 15f;

    [SerializeField]
    private float lifeTime = 3f;

    [SerializeField]
    private float bulletDamage = 20f;

    private ulong ownerClientId;

    private float lifeTimer;

    public void SetOwner(ulong clientId)
    {
        ownerClientId = clientId;
    }

    private void Update()
    {
        if (!IsServer)
            return;

        transform.position += transform.forward * speed * Time.deltaTime;

        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifeTime)
        {
            NetworkObject.Despawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer)
            return;

        Debug.Log("Bullet hit: " + other.gameObject.name);

        var playerHealth = other.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            //stops the player damaging themselves
            if (playerHealth.OwnerClientId == ownerClientId)
                return;

            playerHealth.TakeDamage(bulletDamage);
        }

        NetworkObject.Despawn();
    }
}