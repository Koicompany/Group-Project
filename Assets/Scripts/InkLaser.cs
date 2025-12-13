using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InkLaser : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damage = 10f; // Editable in inspector

    private string targetTag;

    // Initialize the laser at a fire point
    public void Begin(string targetTag)
    {
        this.targetTag = targetTag;

        // Since the laser doesn't move, just auto-destroy after a short time
        StartCoroutine(AutoDestroy());
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(0.5f); // stays for 0.5s
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage); // Use editable damage
            }
            Destroy(gameObject); // Destroy on hit
        }
    }
}
