
using UnityEngine;

public class LocalPlayerShooter : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;
    private LocalInputHandler input;
    private LocalWeaponManager weaponManager;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform gun;

    [SerializeField]
    private float fireRate = 5f;

    private float nextFireTime;

    private void Awake()
    {
        input = GetComponent<LocalInputHandler>();
        weaponManager = GetComponent<LocalWeaponManager>();
    }

    private void Update()
    {
        if (!weaponManager.GunEquipped())
            return;

        if (input.ShootHeld && Time.time >= nextFireTime)
        {
            Shoot();
            playerAnimation.Shoot();

            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, gun.position, gun.rotation);

        var bulletScript = bullet.GetComponent<LocalBullet>();
        bulletScript.SetOwner(gameObject);
    }
}
