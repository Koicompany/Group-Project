using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float ProjectileCooldown;

    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private GameObject projectile;     
    [SerializeField] private Transform firePoint;


    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;


    [Header("Enemy Layer")]
    [SerializeField] private LayerMask enemyLayer;
    private Health enemyHealth;



    private Animator anim;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown)
        {

            Attack();
            cooldownTimer += Time.deltaTime;

        }
        {
            if (Input.GetMouseButton(1) && cooldownTimer > ProjectileCooldown)
                Projectile();
            cooldownTimer += Time.deltaTime;

        }
    }

    private void Attack()
    {

        anim.SetTrigger("attack");
        cooldownTimer = 0;


    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    private void DamageEnemy()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, enemyLayer);

        if (hit.collider != null)
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