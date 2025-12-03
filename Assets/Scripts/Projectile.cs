using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float fixedSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private float direction;


    private Animator anim;
    private BoxCollider2D coll;

    private bool hit;

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
        if (lifetime > resetTime) gameObject.SetActive(false);
    }

    [SerializeField] protected float ProjectileDmg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only react if it hit the enemy or ground
        if (!collision.CompareTag("Player1") && !collision.CompareTag("Ground") && !collision.CompareTag("Player2")) return;

        hit = true;
        collision.GetComponent<Health>()?.TakeDamage(ProjectileDmg);
        coll.enabled = false;

        if (anim != null)
            gameObject.SetActive(false);
    }


    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        coll.enabled = true;

        fixedSpeed = speed;

        float localScaleX = Mathf.Abs(transform.localScale.x) * _direction;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);

    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
