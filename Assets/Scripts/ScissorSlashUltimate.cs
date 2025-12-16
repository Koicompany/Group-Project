using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ScissorSlashUltimate : MonoBehaviour
{
    [Header("Slash Settings")]
    [SerializeField] private float damage = 40f;
    [SerializeField] private float activeTime = 0.15f;      // How long the slash object exists
    [SerializeField] private float damageDelay = 0.2f;      // Delay before dealing damage

    private string enemyTag;
    private HashSet<Health> targets = new HashSet<Health>();
    private BoxCollider2D boxCollider;

    public void Initialize(string enemyTag)
    {
        this.enemyTag = enemyTag;
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        // Start the coroutine for the slash life and delayed damage
        StartCoroutine(SlashRoutine());
    }

    private IEnumerator SlashRoutine()
    {
        // Wait for the damage delay before hitting enemies
        yield return new WaitForSeconds(damageDelay);

        // Apply damage to all targets currently overlapping the slash collider
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(enemyTag))
            {
                Health health = hit.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                    Debug.Log($"Delayed damage applied to {hit.name} for {damage}");
                }
            }
        }

        // Keep the visual for the remainder of activeTime if needed
        float remainingTime = activeTime - damageDelay;
        if (remainingTime > 0)
            yield return new WaitForSeconds(remainingTime);

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (TryGetComponent<BoxCollider2D>(out BoxCollider2D bc))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, bc.size);
        }
    }
#endif
}
