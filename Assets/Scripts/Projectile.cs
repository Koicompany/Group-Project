using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float resetTime = 5f;
    [SerializeField] private float projectileDmg = 10f;

    [Tooltip("If checked, the projectile moves opposite the character's facing direction.")]
    [SerializeField] private bool moveBackward = false;

    private float lifetime;
    private float direction;
    private bool hit;

    private BoxCollider2D coll;
    private Collider2D ownerCollider;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        if (coll == null)
        {
            coll = gameObject.AddComponent<BoxCollider2D>();
            coll.isTrigger = true;
            Debug.LogWarning("Projectile prefab missing BoxCollider2D! Added automatically.");
        }
    }

    public void ActivateProjectile()
    {
        lifetime = 0f;
        hit = false;
        gameObject.SetActive(true);

        if (coll != null)
        {
            coll.enabled = false; // disable briefly to avoid hitting summoner
            StartCoroutine(EnableColliderAfterDelay(0.05f));
        }
    }

    private IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (coll != null)
            coll.enabled = true;
    }

    public void SetOwner(Collider2D owner)
    {
        ownerCollider = owner;
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0f;
        hit = false;
        gameObject.SetActive(true);

        if (coll != null)
            coll.enabled = true;

        direction = moveBackward ? -_direction : _direction;

        float localScaleX = Mathf.Abs(transform.localScale.x) * direction;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Update()
    {
        if (hit) return;

        transform.Translate(speed * Time.deltaTime * direction, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == ownerCollider) return;

        if (!collision.CompareTag("Player1") && !collision.CompareTag("Player2"))
            return;

        hit = true;

        collision.GetComponent<Health>()?.TakeDamage(projectileDmg);

        if (coll != null)
            coll.enabled = false;

        Destroy(gameObject);
    }
}
