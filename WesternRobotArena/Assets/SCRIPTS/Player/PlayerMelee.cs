using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class PlayerMelee : NetworkBehaviour
{
    private InputHandler input;
    private WeaponManager weaponManager;

    [SerializeField]
    private Transform meleePoint;

    [SerializeField]
    private float meleeRange = 1f;

    [SerializeField]
    private float attackCooldown = 0.5f;

    [SerializeField]
    private GameObject meleeIndicator;

    private float nextAttackTime;

    private void Awake()
    {
        input = GetComponent<InputHandler>();
        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        //cant melee while the gun is equipped
        if (!weaponManager.MeleeEquipped())
            return;

        if (input.ShootPressed && Time.time >= nextAttackTime)
        {
            MeleeAttackRpc();

            nextAttackTime = Time.time + attackCooldown;
        }
    }

    [Rpc(SendTo.Server)]
    private void MeleeAttackRpc()
    {
        ShowMeleeIndicatorRpc();

        var hits = Physics.OverlapSphere(meleePoint.position, meleeRange);

        foreach (var hit in hits)
        {
            if (hit.transform.root == transform.root)
                continue;

            Debug.Log(gameObject.name + " melee hit " + hit.gameObject.name);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void ShowMeleeIndicatorRpc()
    {
        StartCoroutine(ShowIndicator());
    }

    private IEnumerator ShowIndicator()
    {
        meleeIndicator.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        meleeIndicator.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (meleePoint == null)
            return;

        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }
}