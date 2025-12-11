using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [HideInInspector] public bool invincible = false;

    [Header("Health")]
    [SerializeField] private float startingHealth = 100f;
    [SerializeField] private float deathDelay = 1.2f;
    [SerializeField] private float hitFlashDuration = 1f;

    public float currentHealth { get; private set; }

    // EVENTS
    public event System.Action<float> OnDamageTaken;
    public static event System.Action<string, float> OnAnyPlayerDamaged;

    private Animator anim;
    private bool dead = false;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private MonoBehaviour[] playerScripts;

    private Color baseColor;

    private void Awake()
    {
        currentHealth = startingHealth;

        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Cache the initial tint (e.g., player 1 or 2)
        baseColor = spriteRenderer.color;

        playerScripts = GetComponents<MonoBehaviour>();
    }

    public float GetStartingHealth() => startingHealth;

    public void TakeDamage(float damage)
    {
        if (dead || invincible) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        // This character took damage
        OnDamageTaken?.Invoke(damage);

        // Inform ALL listeners that a player (with this tag) was damaged
        OnAnyPlayerDamaged?.Invoke(gameObject.tag, damage);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(HitFlash());
        }
        else
        {
            Die();
        }
    }

    private IEnumerator HitFlash()
    {
        if (spriteRenderer == null) yield break;

        float elapsed = 0f;
        float flashInterval = 0.1f;

        while (elapsed < hitFlashDuration)
        {
            // Alternate between red and the player's base color
            spriteRenderer.color =
                (Mathf.FloorToInt(elapsed / flashInterval) % 2 == 0) ? Color.red : baseColor;

            elapsed += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }

        // Ensure we end with the player's base color
        spriteRenderer.color = baseColor;
    }


    private void Die()
    {
        dead = true;
        anim.SetTrigger("die");

        // Disable all scripts except this one
        foreach (var script in playerScripts)
        {
            if (script != this)
                script.enabled = false;
        }

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}
