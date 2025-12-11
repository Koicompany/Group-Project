using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    // Set this in the Inspector to "Player1" or "Player2"
    [SerializeField] private string targetPlayerTag;

    [SerializeField] private Image HealthbarTotal;
    [SerializeField] private Image HealthbarCurrent;

    private Health playerHealth; // Reference to the Health script on the player GameObject
    private float maxHealth;

    private void Awake()
    {
        // 1. Find the GameObject with the specified tag
        GameObject player = GameObject.FindWithTag(targetPlayerTag);

        if (player != null)
        {
            // 2. Get the Health script from the found player
            playerHealth = player.GetComponent<Health>();

            if (playerHealth != null)
            {
                // 3. Get the maximum health using the public getter
                maxHealth = playerHealth.GetStartingHealth();
            }
            else
            {
                Debug.LogError($"Health component not found on object with tag '{targetPlayerTag}'.");
            }
        }
        else
        {
            // This error happens if the player is not in the scene when the healthbar wakes up
            // (e.g., if the player is spawned later)
            Debug.LogError($"GameObject with tag '{targetPlayerTag}' not found in Awake().");
        }
    }

    private void Start()
    {
        if (playerHealth != null)
        {
            // Initialize the bar to full
            HealthbarTotal.fillAmount = 1f;
            UpdateHealthDisplay();
        }
    }

    // Public method called by the Health script when damage is taken
    public void UpdateHealthDisplay()
    {
        if (playerHealth != null && maxHealth > 0)
        {
            // Calculate the fill amount (0 to 1 ratio)
            HealthbarCurrent.fillAmount = playerHealth.currentHealth / maxHealth;
        }
    }
}