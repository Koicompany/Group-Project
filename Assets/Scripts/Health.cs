using UnityEngine;
using System;
using System.Collections;

public class Health : MonoBehaviour
{
    public static event Action<string, float> OnAnyPlayerDamaged;
    public event Action<float> OnDamageTaken;

    [HideInInspector] public bool invincible = false;

    [Header("Health")]
    [SerializeField] private float startingHealth = 100f;
    [SerializeField] private float deathDelay = 1.2f;
    [SerializeField] private float hitFlashDuration = 1f;

    [HideInInspector]
    public float currentHealth;

    private bool dead = false;

    private BoxCollider2D boxCollider;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private MonoBehaviour[] playerScripts;

    private Color baseColor;
    private Coroutine flashRoutine;

    private void Awake()
    {
        currentHealth = startingHealth;

        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        baseColor = spriteRenderer.color;
        playerScripts = GetComponents<MonoBehaviour>();
    }

    public void TakeDamage(float damage)
    {
        if (dead || invincible) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        OnDamageTaken?.Invoke(damage);
        OnAnyPlayerDamaged?.Invoke(gameObject.tag, damage);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");

            if (flashRoutine != null)
                StopCoroutine(flashRoutine);

            flashRoutine = StartCoroutine(HitFlash());
        }
        else
        {
            StartCoroutine(DieRoutine());
        }
    }

    private IEnumerator HitFlash()
    {
        float elapsed = 0f;
        float interval = 0.1f;

        while (elapsed < hitFlashDuration)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(interval);

            spriteRenderer.color = baseColor;
            yield return new WaitForSeconds(interval);

            elapsed += interval * 2f;
        }

        spriteRenderer.color = baseColor;
        flashRoutine = null;
    }

    private IEnumerator DieRoutine()
    {
        if (dead) yield break;
        dead = true;

        // Play death animation
        anim.SetTrigger("die");

        // Disable gameplay scripts immediately
        foreach (var script in playerScripts)
        {
            if (script != this)
                script.enabled = false;
        }

        // Wait for death animation duration
        yield return new WaitForSeconds(deathDelay);

        // Disable collider and remove object
        boxCollider.enabled = false;
        Destroy(gameObject);
    }

    public float GetStartingHealth()
    {
        return startingHealth;
    }
}
