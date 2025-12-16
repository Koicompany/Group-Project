using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 10f;
    [SerializeField] private float destroyDelay = 10f;

    private string enemyTag;
    private float damage;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Initialize(string enemyTag, float damage)
    {
        this.enemyTag = enemyTag;
        this.damage = damage;

        if (animator != null)
        {
            animator.SetTrigger("rockfall");
        }

        Destroy(gameObject, destroyDelay);
    }

    private void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(enemyTag))
            return;

        Health health = collision.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
