using UnityEngine;

public class ArchingProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float resetTime = 3f;
    [SerializeField] private float projectileDmg = 10f;
    [SerializeField] private float launchAngleDegrees = 45f;

    private float lifetime;
    private bool hit;
    private float direction;

    private Rigidbody2D rb;
    private BoxCollider2D coll;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        // Prevent projectile from falling straight down if launched without SetDirection()
        rb.gravityScale = 1f;
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        hit = false;
        lifetime = 0f;

        coll.enabled = true;

        // Flip the projectile visually
        float scaleX = Mathf.Abs(transform.localScale.x) * direction;
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

        // Apply arcing velocity
        float angleRadians = launchAngleDegrees * Mathf.Deg2Rad;

        Vector2 velocity = new Vector2(
            Mathf.Cos(angleRadians) * speed * direction,
            Mathf.Sin(angleRadians) * speed
        );

        rb.linearVelocity = velocity;
    }

    private void Update()
    {
        if (hit) return;

        lifetime += Time.deltaTime;
        if (lifetime >= resetTime)
        {
            Destroy(gameObject); // since you're instantiating
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only deactivate on Player1, Player2, or Ground
        if (!collision.CompareTag("Player1") &&
            !collision.CompareTag("Player2") &&
            !collision.CompareTag("Ground"))
            return;

        hit = true;

        // Damage if the target has a Health component
        collision.GetComponent<Health>()?.TakeDamage(projectileDmg);

        coll.enabled = false;
        rb.linearVelocity = Vector2.zero;

        Destroy(gameObject); // since you use Instantiate
    }
}
