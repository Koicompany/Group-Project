using UnityEngine;

public class ArrowPlayerAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float projectileCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePoint;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Enemy Tag")]
    [SerializeField] private string enemyTag = "Enemy";  // Changed from 'tag' type to string

    private Animator anim;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;  // Move this outside the input checks

        // Melee attack - key M
        if (Input.GetKeyDown(KeyCode.M) && cooldownTimer > attackCooldown)
        {
            Attack();
        }

        // Ranged attack - key Comma (,)
        if (Input.GetKeyDown(KeyCode.Comma) && cooldownTimer > projectileCooldown)
        {
            Projectile(); // renamed from Projectile() for clarity
        }
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        DamageEnemy(); // Call damage function when attacking
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamageEnemy()
    {
        // BoxCast in front of player
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y),
            0f,
            Vector2.left,
            0f
        );

        if (hit.collider != null && hit.collider.CompareTag(enemyTag))  // Check tag instead of layer
        {
            Health enemyHealth = hit.collider.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    private void Projectile()
    {
        cooldownTimer = 0;

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

        GameObject newProjectile = Instantiate(projectile, firePoint.position, Quaternion.identity);
        return newProjectile;
    }
}
