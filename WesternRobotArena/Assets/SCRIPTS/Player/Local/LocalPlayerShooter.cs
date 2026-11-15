
using UnityEngine;

public class LocalPlayerShooter : MonoBehaviour
{
    private LocalInputHandler input;
    private LocalWeaponManager weaponManager;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform gun;

    [SerializeField]
    private float fireRate = 5f;

    private float nextFireTime;
    [Header("Animation")]
    [SerializeField] private PlayerAnimation playerAnimation;

    [Header("Audio")]
    [SerializeField] private AudioClip rifleShootSound;
    private AudioSource audioSource;

    private void Awake()
    {
        input = GetComponent<LocalInputHandler>();
        weaponManager = GetComponent<LocalWeaponManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!weaponManager.GunEquipped())
        {
            playerAnimation.Shoot(false);
            return;
        }

        playerAnimation.Shoot(input.ShootHeld);
        if (input.ShootHeld && Time.time >= nextFireTime)
        {
            //Shoot();
            // Delay the shooting to sync with the animation
            Invoke(nameof(Shoot), 0.3f);

            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, gun.position, gun.rotation);
        
        audioSource.PlayOneShot(rifleShootSound);
        audioSource.volume = 0.3f;

        var bulletScript = bullet.GetComponent<LocalBullet>();
        bulletScript.SetOwner(gameObject);
    }
}
