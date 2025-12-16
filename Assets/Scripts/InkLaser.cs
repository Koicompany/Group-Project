using UnityEngine;

public class InkLaser : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damagePerTick = 2f;
    [SerializeField] private float tickRate = 0.25f;

    private string targetTag;
    private float timer;

    public void Initialize(string targetTag)
    {
        this.targetTag = targetTag;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag))
            return;

        timer += Time.deltaTime;

        if (timer >= tickRate)
        {
            timer = 0f;

            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damagePerTick);
            }
        }
    }
}
