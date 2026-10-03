using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private EnemyStats enemystats;

    private float nextAttackTime = 0f;
    public void Initialise (EnemyStats stats)
    {
        enemystats = stats;
    }

    public void TryAttack(Transform target)
    {
        if(target == null)
            return; 
        if(enemystats == null)
            return;

        if(Time.time < nextAttackTime)
            return;

        float distance = Vector3.Distance(transform.position, target.position);

        if(distance > enemystats.attackRange)
            return;

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();

        if(playerHealth == null)
            return;

        playerHealth.TakeDamage(enemystats.attackDamage);

        nextAttackTime = Time.time + enemystats.attackCooldown;


    }
}
