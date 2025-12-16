using UnityEngine;

public class BoulderDamage : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float lifeTime = 3f;

    private string enemyTag;

    public void Init(string enemyTag)
    {
        this.enemyTag = enemyTag;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(enemyTag)) return;

        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
