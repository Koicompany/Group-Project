using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private string targetPlayerTag = "Player1";
    [SerializeField] private Image HealthbarCurrent;

    private Health playerHealth;
    private float maxHealth;

    private void Start()
    {
        InvokeRepeating(nameof(TryFindPlayer), 0f, 0.25f);
    }

    void TryFindPlayer()
    {
        if (playerHealth != null) return;

        GameObject player = GameObject.FindWithTag(targetPlayerTag);

        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();

            if (playerHealth != null)
            {
                maxHealth = playerHealth.GetStartingHealth();
                UpdateHealthDisplay();
                CancelInvoke(nameof(TryFindPlayer));
            }
        }
    }

    private void Update()
    {
        if (playerHealth == null || maxHealth <= 0) return;

        HealthbarCurrent.fillAmount = playerHealth.currentHealth / maxHealth;
    }

    private void UpdateHealthDisplay()
    {
        if (playerHealth != null && maxHealth > 0)
        {
            HealthbarCurrent.fillAmount = playerHealth.currentHealth / maxHealth;
        }
    }
}
