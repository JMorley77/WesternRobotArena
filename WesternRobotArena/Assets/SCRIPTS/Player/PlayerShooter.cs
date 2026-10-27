using Unity.Netcode;
using UnityEngine;

public class PlayerShooter : NetworkBehaviour
{
    private InputHandler input;
    private WeaponManager weaponManager;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform gun;

    [SerializeField]
    private float fireRate = 5f;

    private float nextFireTime;

    private void Awake()
    {
        input = GetComponent<InputHandler>();
        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        if (!weaponManager.GunEquipped())
            return;

        if (input.ShootHeld && Time.time >= nextFireTime)
        {
            ShootRpc();

            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    [Rpc(SendTo.Server)]
    private void ShootRpc()
    {
        var bullet = Instantiate( bulletPrefab, gun.position, gun.rotation);

        var bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetOwner(OwnerClientId);

        bullet.GetComponent<NetworkObject>().Spawn();
    }
}