using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float resetTime = 5f;
    [SerializeField] private float projectileDmg = 10f;

    [Header("Homing Settings")]
    [Tooltip("Check to target Player1, uncheck for Player2.")]
    [SerializeField] private bool targetPlayer1 = true;

    private float lifetime;
    private bool hit;
    private Transform target;
    private BoxCollider2D coll;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        if (coll == null)
        {
            coll = gameObject.AddComponent<BoxCollider2D>();
            coll.isTrigger = true;
        }
    }

    /// <summary>
    /// Activates the projectile
    /// </summary>
    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;

        // Set target based on boolean
        string targetTag = targetPlayer1 ? "Player1" : "Player2";
        GameObject targetObj = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObj != null)
            target = targetObj.transform;
    }

    private void Update()
    {
        if (hit) return;

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            Destroy(gameObject);
            return;
        }

        if (target != null)
        {
            // Homing movement
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            float rotateAmount = Vector3.Cross(directionToTarget, transform.right).z;
            transform.Rotate(0, 0, -rotateAmount * 200f * Time.deltaTime);
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only collide with the intended player or ground
        if ((targetPlayer1 && collision.CompareTag("Player1")) || (!targetPlayer1 && collision.CompareTag("Player2")))
        {
            hit = true;
            collision.GetComponent<Health>()?.TakeDamage(projectileDmg);
            coll.enabled = false;
            Destroy(gameObject);
        }
    }
}
