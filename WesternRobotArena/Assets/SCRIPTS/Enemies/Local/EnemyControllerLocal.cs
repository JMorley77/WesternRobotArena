using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerLocal : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chasing,
        Attacking,
        Dead
    }

    [Header("Enemy Stats")]
    [SerializeField] private EnemyStats enemyStats;

    private NavMeshAgent agent;
    private Transform target;
    private EnemyCombat enemyCombat;
    private EnemyState currentState = EnemyState.Idle;

    private void Awake()
    {
        enemyCombat = GetComponent<EnemyCombat>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (agent == null || enemyStats == null)
        {
            Debug.LogError("Enemy Agent or Enemy Stats is missing!");
            enabled = false;
            return;
        }

        agent.speed = enemyStats.moveSpeed;
        agent.stoppingDistance = enemyStats.attackRange;

        if (enemyCombat != null)
            enemyCombat.Initialise(enemyStats);
    }

    private void Update()
    {
        if (currentState == EnemyState.Dead)
            return;

        if (target == null)
        {
            currentState = EnemyState.Idle;
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            target.position
        );

        if (distance <= enemyStats.attackRange)
        {
            StopMoving();
            currentState = EnemyState.Attacking;
            UpdateAttacking();
        }
        else
        {
            currentState = EnemyState.Chasing;
            ChaseTarget();
        }
    }

    public void SetTarget(Transform targetPlayer)
    {
        target = targetPlayer;
    }

    private void ChaseTarget()
    {
        if (target == null || agent == null)
            return;

        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    private void StopMoving()
    {
        if (agent == null)
            return;

        agent.isStopped = true;
        agent.ResetPath();
    }

    public void UpdateAttacking()
    {
        if (target == null || enemyCombat == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            target.position
        );

        if (distance > enemyStats.attackRange)
        {
            currentState = EnemyState.Chasing;
            return;
        }

        enemyCombat.TryAttack(target);
    }

    public void Die()
    {
        if (currentState == EnemyState.Dead)
            return;

        currentState = EnemyState.Dead;

        StopMoving();

        Destroy(gameObject);
    }
}