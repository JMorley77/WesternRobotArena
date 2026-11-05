
using UnityEngine;
using System.Collections;

public class LocalPlayerMelee : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;

    private LocalInputHandler input;
    private LocalWeaponManager weaponManager;

    [SerializeField]
    private Transform meleePoint;

    [SerializeField]
    private float meleeRange = 1f;

    [SerializeField]
    private float attackCooldown = 0.5f;

    [SerializeField]
    private float meleeDamage = 25f;

    [SerializeField]
    private GameObject meleeIndicator;

    private float nextAttackTime;

    private void Awake()
    {
        input = GetComponent<LocalInputHandler>();
        weaponManager = GetComponent<LocalWeaponManager>();
    }

    private void Update()
    {
        // Can't melee while the gun is equipped
        if (!weaponManager.MeleeEquipped())
            return;

        if (input.ShootPressed && Time.time >= nextAttackTime)
        {
            MeleeAttack();

            playerAnimation.SwordAttack();

            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void MeleeAttack()
    {
        StartCoroutine(ShowIndicator());

        var hits = Physics.OverlapSphere(meleePoint.position, meleeRange);

        foreach (var hit in hits)
        {
            if (hit.transform.root == transform.root)
                continue;

            var playerHealth = hit.GetComponentInParent<PlayerHealthLocal>();
            var enemyHealth = hit.GetComponentInParent<EnemyHealthLocal>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);

                Debug.Log(gameObject.name + " melee hit " + hit.gameObject.name);
                break;
            }

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(meleeDamage);
            }
        }
    }

    private IEnumerator ShowIndicator()
    {
        meleeIndicator.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        meleeIndicator.SetActive(false);
    }
}
