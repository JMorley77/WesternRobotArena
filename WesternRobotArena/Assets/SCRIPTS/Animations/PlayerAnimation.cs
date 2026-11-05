using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Animation Settings")]
    public string speedParameter = "SpeedParameter";
    public string attackAnimation = "AttackAnimation";
    public string shootingAnimation = "Shoot";
    public string swordAttackAnimation = "SwordAttack";

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        animator.SetFloat("Speed", 0f);
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

    public void SetMovementSpeed(float speed)
    {
        animator.SetFloat(speedParameter, speed);
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

    public void ResetAnimations()
    {
        animator.SetFloat(speedParameter, 0f);
        animator.ResetTrigger(attackAnimation);
        animator.SetBool(shootingAnimation, false);
        animator.ResetTrigger(swordAttackAnimation);
    }
}
