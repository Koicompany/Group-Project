using UnityEngine;
using System.Collections;

public class ScissorSlashUltimate : MonoBehaviour
{
    [Header("Slash Settings")]
    [SerializeField] private float damage = 40f;
    [SerializeField] private float range = 2.2f;
    [SerializeField] private float activeTime = 0.15f;

    [Header("Hitbox")]
    [SerializeField] private Vector2 hitboxSize = new Vector2(2.5f, 1.5f);
    [SerializeField] private LayerMask playerLayer;

    [Header("Visuals")]
    [SerializeField] private GameObject slashEffectPrefab;

    private string enemyTag;
    private bool facingRight;

    // CALLED BY UltimateMoveManager
    public void Initialize(string enemyTag, bool facingRight)
    {
        this.enemyTag = enemyTag;
        this.facingRight = facingRight;

        ApplyFacing();
        StartCoroutine(SlashRoutine());
    }

    private void ApplyFacing()
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private IEnumerator SlashRoutine()
    {
        ExecuteSlash();

        yield return new WaitForSeconds(activeTime);

        Destroy(gameObject);
    }

    private void ExecuteSlash()
    {
        Vector2 center = (Vector2)transform.position +
                         (facingRight ? Vector2.right : Vector2.left) * range * 0.5f;

        if (slashEffectPrefab != null)
        {
            Instantiate(
                slashEffectPrefab,
                center,
                Quaternion.identity
            );
        }

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            center,
            hitboxSize,
            0f,
            playerLayer
        );

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag(enemyTag))
                continue;

            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}