using System;
using TMPro;

using UnityEngine;

public class PlayerHealthLocal : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private PlayerAnimation playerAnimation;
    private LocalCharacterMover character;

    [Header("Audio")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;

    [Header("Damage Popup")]
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private Vector3 popupOffset = new Vector3(5f, 10f, 0f);
    [SerializeField] private float popupLifetime = 0.8f;

    private bool isDead = false;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => isDead;

    public event Action<PlayerHealthLocal> OnPlayerDied;

    [Header("Invincibility")]
    private bool isInvincible;
    private float invincibilityTimer;

    private void Start()
    {
        currentHealth = maxHealth;
        character = GetComponent<LocalCharacterMover>();
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (isInvincible)
            return;

        invincibilityTimer -= Time.deltaTime;

        if (invincibilityTimer <= 0)
        {
            isInvincible = false;
        }

        if (healthText != null)
            healthText.text = "Health: " + currentHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isInvincible || damage <= 0f)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);

        ShowDamagePopup(damage);

        audioSource.PlayOneShot(hitSound);
        Debug.Log(gameObject.name + " Health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead)
            return;
        if (character != null) character.enabled = false;
        isDead = true;
        currentHealth = 0f;
        playerAnimation.Die();
        audioSource.PlayOneShot(deathSound);
        Debug.Log(gameObject.name + " died!");


    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        if (character != null) character.enabled = true;

    }
    public void Heal(float amount)
    {
        if (isDead || amount <= 0f)
            return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        Debug.Log(gameObject.name + " healed. Health: " + currentHealth);
    }

    public void Invincibility(float duration)
    {
        isInvincible = true;
        invincibilityTimer = duration;

        Debug.Log(gameObject.name + " is now invincible for " + duration + " seconds!");
    }

    private void ShowDamagePopup(float damage)
    {
        if (damagePopupPrefab == null) return;

        // Small random sideways offset so rapid hits don't stack exactly on top of each other
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0f, 0f);

        Quaternion rot = Camera.main != null
            ? Camera.main.transform.rotation
            : Quaternion.identity;

        GameObject popup = Instantiate(
            damagePopupPrefab,
            transform.position + popupOffset + randomOffset,
            rot
        );

        TMP_Text popupText = popup.GetComponentInChildren<TMP_Text>();

        if (popupText != null)
        {
            popupText.text = damage.ToString("0");
            popupText.color = Color.red;
        }

        Destroy(popup, popupLifetime);
    }
}
