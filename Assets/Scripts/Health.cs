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

    public float currentHealth;
    private bool dead = false;

    private BoxCollider2D boxCollider;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private MonoBehaviour[] playerScripts;

  
    private Color baseColor;          
    private Coroutine flashRoutine;   

    public float GetStartingHealth()
    {
        return startingHealth;
    }

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

            // Stop old flash if already running
            if (flashRoutine != null)
                StopCoroutine(flashRoutine);

            flashRoutine = StartCoroutine(HitFlash());
        }
        else
        {
            Die();
        }
    }

    private IEnumerator HitFlash()
    {
        float elapsed = 0f;
        float interval = 0.1f;

        while (elapsed < hitFlashDuration)
        {
            // flash red
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(interval);

            // back to base tint
            spriteRenderer.color = baseColor;
            yield return new WaitForSeconds(interval);

            elapsed += interval * 2f;
        }

        // Final restore (just in case)
        spriteRenderer.color = baseColor;
        flashRoutine = null;
    }

    private void Die()
    {
        if (dead) return;
        dead = true;
        anim.SetTrigger("die");
        boxCollider.enabled = false;

        // Disable other player scripts
        foreach (var script in playerScripts)
        {
            if (script != this) script.enabled = false;
        }

        Destroy(gameObject, deathDelay);
    }
}
