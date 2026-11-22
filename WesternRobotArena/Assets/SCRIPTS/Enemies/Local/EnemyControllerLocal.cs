using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerLocal : MonoBehaviour
{
    public enum EnemyState
    {
        Wandering,
        Chasing,
        Attacking,
        Dead
    }

    [Header("Enemy Stats")]
    [SerializeField] private EnemyStats enemyStats;

    [Header("Behaviour")]
    [SerializeField] private float detectionRange = 10f;   
    [SerializeField] private float wanderRadius = 12f;     
    [SerializeField] private float wanderInterval = 4f;

    public Animator animator;
    private NavMeshAgent agent;
    private Transform target;
    private EnemyCombat enemyCombat;
    private EnemyState currentState = EnemyState.Wandering;

    private Vector3 startPosition;
    private float wanderTimer;

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

        startPosition = transform.position;
        agent.speed = enemyStats.moveSpeed;
        agent.stoppingDistance = enemyStats.attackRange;

        if (enemyCombat != null)
        {
            enemyCombat.Initialise(enemyStats);
        }

        wanderTimer = wanderInterval; 
    }

    private void Update()
    {
        if (currentState == EnemyState.Dead)
            return;


        if (target == null)
        {
            Wander();
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= enemyStats.attackRange)
        {
            currentState = EnemyState.Attacking;
            Attack();
        }
        else if (distance <= detectionRange)
        {
            currentState = EnemyState.Chasing;
            Chase();
        }
        else
        {
            currentState = EnemyState.Wandering;
            Wander();
        }
    }

    public void SetTarget(Transform targetPlayer)
    {
        target = targetPlayer;
    }

    private void Wander()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderInterval)
        {
            wanderTimer = 0f;

            Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
            Vector3 randomPoint = startPosition + new Vector3(randomOffset.x, 0f, randomOffset.y);

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                agent.isStopped = false;
                agent.SetDestination(hit.position);
            }
        }

        animator.SetBool("Walking", agent.velocity.magnitude > 0.1f);
    }

    private void Chase()
    {
        animator.SetBool("Walking", true);
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    private void Attack()
    {
        animator.SetBool("Walking", false);
        animator.SetTrigger("Attack");
        agent.isStopped = true;


        Vector3 direction = target.position - transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (enemyCombat != null)
        {
            enemyCombat.TryAttack(target);
        }
    }

    public void Die()
    {
        if (currentState == EnemyState.Dead)
            return;

        currentState = EnemyState.Dead;
        agent.isStopped = true;
        agent.ResetPath();

        //Destroy(gameObject);
    }
}