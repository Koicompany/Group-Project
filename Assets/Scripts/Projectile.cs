using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    [SerializeField] private float projectileDmg;

    private float lifetime;
    private float direction;
    private bool hit;

    private Animator anim;
    private BoxCollider2D coll;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }

    private void Update()
    {
        if (hit) return;

        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only destroy projectile if it hits Player1, Player2, or Ground
        if (!collision.CompareTag("Player1") && !collision.CompareTag("Player2") && !collision.CompareTag("Ground"))

            return;

        hit = true;

        // Deal damage if the target has a Health component
        collision.GetComponent<Health>()?.TakeDamage(projectileDmg);

        coll.enabled = false;

        Destroy(gameObject);

    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        coll.enabled = true;

        float localScaleX = Mathf.Abs(transform.localScale.x) * _direction;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

}
