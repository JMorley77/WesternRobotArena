using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private EnemyStats enemyStats;

    private float nextAttackTime = 0f;

    public void Initialise(EnemyStats stats)
    {
        enemyStats = stats;
    }

    public void TryAttack(Transform target)
    {
        if (target == null || enemyStats == null)
            return;

        if (Time.time < nextAttackTime)
            return;

        float distance = Vector3.Distance(
            transform.position,
            target.position
        );

        if (distance > enemyStats.attackRange)
            return;

        // Check for multiplayer health first.
        PlayerHealth onlineHealth = target.GetComponent<PlayerHealth>();

        if (onlineHealth != null)
        {
            onlineHealth.TakeDamage(enemyStats.attackDamage);
        }
        else
        {
            //check for local health.
            PlayerHealthLocal localHealth =target.GetComponent<PlayerHealthLocal>();

            if (localHealth == null)
                return;

            localHealth.TakeDamage(enemyStats.attackDamage);
        }

        nextAttackTime = Time.time + enemyStats.attackCooldown;
    }
}
