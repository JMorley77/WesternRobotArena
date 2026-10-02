using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : NetworkBehaviour
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
    private ulong targetPlayerId;
    private Transform target;

    private EnemyState currentState= EnemyState.Idle;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            agent.enabled = false;
            return;
        }

        agent.speed = enemyStats.moveSpeed;
        agent.stoppingDistance = enemyStats.attackRange;
    }

    private void Update()
    {
        if (!IsServer)
            return;

        if(currentState == EnemyState.Dead)
            return;

        if (target == null)
        {
            FindTarget();
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= enemyStats.attackRange)
        {
            StopMoving();
            currentState = EnemyState.Attacking;
        }
        else
        {
            currentState = EnemyState.Chasing;
            ChaseTarget();
        }
    }



    public void SetTarget(ulong playerId)
    {
        if(!IsServer)
            return;
        targetPlayerId = playerId;
        FindTarget();
    }

    private void FindTarget()
    {
        if(!NetworkManager.Singleton.ConnectedClients.TryGetValue(targetPlayerId, out var client))
            return;

        if(client.PlayerObject == null)
            return;

        target = client.PlayerObject.transform;
    }

    private void ChaseTarget()
    {
        if(target == null)
            return;

        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    private void StopMoving()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void Die()
    {
        if(!IsServer)
            return;

        currentState = EnemyState.Dead;
        StopMoving();
        Destroy(gameObject);
    }
}
