using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkLaser : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damagePerTick = 2f;
    [SerializeField] private float tickRate = 0.25f;

    private string targetTag;
    private HashSet<Health> targets = new HashSet<Health>();

    public void Initialize(string targetTag)
    {
        this.targetTag = targetTag;
        StartCoroutine(DamageRoutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
                targets.Add(health);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
                targets.Remove(health);
        }
    }

    private IEnumerator DamageRoutine()
    {
        while (true)
        {
            foreach (var health in targets)
            {
                if (health != null)
                    health.TakeDamage(damagePerTick);
            }
            yield return new WaitForSeconds(tickRate);
        }
    }
}
