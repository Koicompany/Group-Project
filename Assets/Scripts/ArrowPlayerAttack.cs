using UnityEngine;
using System.Collections;

public class ArrowPlayerAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float projectileCooldown = 1f;
    [SerializeField] private float meleeDelay = 0.3f;       // Delay before melee deals damage
    [SerializeField] private float projectileDelay = 0.5f;  // Delay before projectile fires
    [SerializeField] private float range = 1f;
    [SerializeField] private int damage = 10;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePoint;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance = 1f;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Enemy Tag")]
    [SerializeField] private string enemyTag = "Enemy";

    private Animator anim;
    private float cooldownTimer = Mathf.Infinity;
    private bool isAttacking = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Melee attack - key M
        if (Input.GetKeyDown(KeyCode.M) && cooldownTimer > attackCooldown && !isAttacking)
        {
            StartCoroutine(MeleeAttack());
        }

        // Ranged attack - key Comma (,)
        if (Input.GetKeyDown(KeyCode.Comma) && cooldownTimer > projectileCooldown && !isAttacking)
        {
            StartCoroutine(RangedAttack());
        }
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        cooldownTimer = 0f;

        anim.SetTrigger("attack");

        yield return new WaitForSeconds(meleeDelay); // delay before damage

        DamageEnemy();

        isAttacking = false;
    }

    private IEnumerator RangedAttack()
    {
        isAttacking = true;
        cooldownTimer = 0f;

        anim.SetTrigger("longRange");

        yield return new WaitForSeconds(projectileDelay); // delay before firing projectile

        FireProjectile();

        isAttacking = false;
    }

    private void DamageEnemy()
    {
        if (boxCollider == null) return;

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y),
            0f,
            Vector2.left,
            0f
        );

        if (hit.collider != null && hit.collider.CompareTag(enemyTag))
        {
            hit.collider.GetComponent<Health>()?.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.red;

        // Calculate the cast box position & size (matching your BoxCast)
        Vector3 castCenter = boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance;

        Vector3 castSize = new Vector3(
            boxCollider.bounds.size.x * range,
            boxCollider.bounds.size.y,
            1f
        );

        Gizmos.DrawWireCube(castCenter, castSize);
    }

    private void FireProjectile()
    {
        GameObject newProjectile = FindProjectile();
        if (newProjectile != null)
        {
            newProjectile.transform.position = firePoint.position;
            newProjectile.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }

    private GameObject FindProjectile()
    {
        if (projectile == null || firePoint == null)
        {
            Debug.LogWarning("Projectile prefab or firePoint not set!");
            return null;
        }
        return Instantiate(projectile, firePoint.position, Quaternion.identity);
    }
}
