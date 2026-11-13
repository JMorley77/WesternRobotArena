using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Animation Settings")]
    public string speedParameter = "SpeedParameter";
    public string DirectionAnimation = "Direction";
    public string attackAnimation = "AttackAnimation";
    public string shootingAnimation = "Shoot";
    public string swordAttackAnimation = "SwordAttack";
    public string deathAnimation = "IsDead";
    public string pickupAnimation = "Pickup";

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        animator.SetFloat("Speed", 0f);
        animator.SetFloat("Direction", 0f);
    }


    public void Idle()
    {
        animator.SetFloat(speedParameter, 0f);
    }
    public void Walk()
    {
        animator.SetFloat(speedParameter, 0.5f);
    }

    public void Run()
    {
        animator.SetFloat(speedParameter, 1f);
    }

    public void StrafeLeft()
    {
        animator.SetFloat(DirectionAnimation, -1f);
    }

    public void StrafeRight()
    {
        animator.SetFloat(DirectionAnimation, 1f);
    }
    public void SetMovementSpeed(float speed)
    {
        animator.SetFloat(speedParameter, speed);
    }

    public void Pickup()
    {
        animator.SetTrigger(pickupAnimation);
    }

    public void Attack()
    {
        animator.SetTrigger(attackAnimation);
    }

    public void Shoot(bool shooting)
    {
        animator.SetBool(shootingAnimation, shooting);
    }

    public void SwordAttack()
    {
        animator.SetTrigger(swordAttackAnimation);
    }

    public void Die()
    {
        animator.SetTrigger(deathAnimation);
    }
    public void SetMovement(float direction, float forward, float speed)
    {
        animator.SetFloat("Direction", direction, 0.05f, Time.deltaTime);
        animator.SetFloat("Forward", forward, 0.05f, Time.deltaTime);
        animator.SetFloat("Speed", speed, 0.05f, Time.deltaTime);
    }

    public void SetLocomotionSpeed(float multiplier)
    {
        animator.SetFloat("LocoSpeed", multiplier, 0.1f, Time.deltaTime);
    }
    public void ResetAnimations()
    {
        animator.SetFloat(speedParameter, 0f);
        animator.SetFloat(DirectionAnimation, 0f);
        animator.ResetTrigger(attackAnimation);
        animator.SetBool(shootingAnimation, false);
        animator.ResetTrigger(swordAttackAnimation);
    }
}
