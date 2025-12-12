using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private string targetPlayerTag;

    [SerializeField] private Image HealthbarTotal;
    [SerializeField] private Image HealthbarCurrent;

    private Health playerHealth;
    private float maxHealth;
    private bool isTracking = false;

    private void Start()
    {
        // Try to find the player immediately, and start tracking if not found
        StartCoroutine(FindAndTrackPlayer());
    }

    private IEnumerator FindAndTrackPlayer()
    {
        while (!isTracking)
        {
            GameObject player = GameObject.FindWithTag(targetPlayerTag);
            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                {
                    maxHealth = playerHealth.GetStartingHealth();


                    // Subscribe to damage event
                    playerHealth.OnDamageTaken += OnHealthChanged;

                    // Initialize the health bar
                    if (HealthbarTotal != null) HealthbarTotal.fillAmount = 1f;
                    UpdateHealthDisplay();

                    isTracking = true;
                    yield break; // stop the coroutine once tracking starts
                }
            }

            // Wait a short time before trying again
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDamageTaken -= OnHealthChanged;
        }
    }

    private void OnHealthChanged(float damage)
    {
        UpdateHealthDisplay();
    }

    public void UpdateHealthDisplay()
    {
        if (playerHealth != null && maxHealth > 0 && HealthbarCurrent != null)
        {
            HealthbarCurrent.fillAmount = playerHealth.currentHealth / maxHealth;
        }
    }
}
