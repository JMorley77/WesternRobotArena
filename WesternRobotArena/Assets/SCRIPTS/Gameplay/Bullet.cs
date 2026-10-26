using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField]
    private float speed = 15f;

    [SerializeField]
    private float lifeTime = 3f;

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

        var hitNetworkObject = other.GetComponentInParent<NetworkObject>();

        //stops the bullet hitting the player who fired it
        if (hitNetworkObject != null && hitNetworkObject.OwnerClientId == ownerClientId)
        {
            return;
        }

        NetworkObject.Despawn();
    }
}