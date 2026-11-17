
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalPlayerShooter : MonoBehaviour
{
    private LocalInputHandler input;
    private LocalWeaponManager weaponManager;
    private PlayerAnimation animation;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform gun;

    [SerializeField]
    private float fireRate = 5f;

    private float nextFireTime;

    [Header("Ammo")]
    [SerializeField] private int magSize = 5;
    [SerializeField] private int currentAmmo = 5;
    //[SerializeField] private int reloadTime = 2;

    private bool isReloading;
    private float reloadTimer;

    [Header("Animation")]
    [SerializeField] private PlayerAnimation playerAnimation;

    [Header("Audio")]
    [SerializeField] private AudioClip rifleShootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip emptyMagSound;
    private AudioSource audioSource;

    private void Awake()
    {
        input = GetComponent<LocalInputHandler>();
        weaponManager = GetComponent<LocalWeaponManager>();
        audioSource = GetComponent<AudioSource>();
        animation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        if (isReloading)
        {
            if (Time.time >= reloadTimer)
            {
                currentAmmo = magSize;
                isReloading = false;
            }
            return;
        }

        if (input.ReloadPressed)
        {
            if (currentAmmo < magSize)
            {
                isReloading = true;
                reloadTimer = Time.time + reloadSound.length;
                audioSource.PlayOneShot(reloadSound);
                animation.Shoot(false);
                animation.Reload();

            }
            return;
        }
        
        if (!weaponManager.GunEquipped())
        {
            playerAnimation.Shoot(false);
            return;
        }

        playerAnimation.Shoot(input.ShootHeld);
        if (input.ShootHeld && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                // Delay the shooting to sync with the animation
                Invoke(nameof(Shoot), 0.3f);
                //Shoot();

                currentAmmo--;

                nextFireTime = Time.time + 1f / fireRate;
            }
            else
            {
                audioSource.PlayOneShot(emptyMagSound);

                nextFireTime = Time.time + 1f / fireRate;
            }

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
