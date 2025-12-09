using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [HideInInspector] public bool invincible = false;

    [Header("Health")]
    [SerializeField] private float startingHealth = 100f;
    [SerializeField] private float deathDelay = 1.2f;  // time before despawning
    [SerializeField] private float hitFlashDuration = 1f; // duration of red/white flash

    public float currentHealth { get; private set; }

    private Animator anim;
    private bool dead = false;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    // Store all scripts that should disable upon death
    private MonoBehaviour[] playerScripts;

    private void Awake()
    {
        currentHealth = startingHealth;
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Cache every script on the player EXCEPT this one
        playerScripts = GetComponents<MonoBehaviour>();
    }

    public float GetStartingHealth()
    {
        return startingHealth;
    }

    public void TakeDamage(float damage)
    {
        if (dead || invincible) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

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

        Color originalColor = spriteRenderer.color; // Save the original color
        float elapsed = 0f;
        float flashInterval = 0.1f; // Time per flash

        while (elapsed < hitFlashDuration)
        {
            // Alternate between red and white
            spriteRenderer.color = (Mathf.FloorToInt(elapsed / flashInterval) % 2 == 0) ? Color.red : Color.white;

            elapsed += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }

        // Restore original color
        spriteRenderer.color = originalColor;
    }



    private void Die()
    {
        dead = true;

        anim.SetTrigger("die");

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
