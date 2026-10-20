using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private InputHandler input;

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
    }

    private void Update()
    {
        if (input.ShootHeld && Time.time >= nextFireTime)
        {
            Shoot();

            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(
            bulletPrefab,
            gun.position,
            gun.rotation
        );

        bullet.GetComponent<Bullet>().SetOwner(gameObject);
    }
}