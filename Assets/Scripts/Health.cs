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

        Color originalColor = spriteRenderer.color; // Save the current tint

        float elapsed = 0f;
        bool toggle = true;

        while (elapsed < hitFlashDuration)
        {
            spriteRenderer.color = toggle ? Color.red : originalColor;
            toggle = !toggle;
            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        spriteRenderer.color = originalColor; // Restore original tint
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
