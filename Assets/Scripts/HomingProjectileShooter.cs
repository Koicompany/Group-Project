using UnityEngine;

public class HomingProjectileShooter : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private KeyCode shootKey = KeyCode.Y; // Configurable in inspector
    [SerializeField] private bool targetPlayer1 = true;
    [SerializeField] private float projectileCooldown = 1f; // Cooldown in seconds

    private Animator anim;
    private float cooldownTimer = 0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Update the cooldown timer
        cooldownTimer += Time.deltaTime;

        // Check input and cooldown
        if (Input.GetKeyDown(shootKey) && cooldownTimer >= projectileCooldown)
        {
            anim.SetTrigger("Special");
            FireProjectile();
            cooldownTimer = 0f; // Reset cooldown after firing
        }
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject newProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        HomingProjectile projScript = newProjectile.GetComponent<HomingProjectile>();
        if (projScript != null)
        {
            projScript.ActivateProjectile();

            // Set which player to target
            projScript.GetType().GetField("targetPlayer1",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(projScript, targetPlayer1);
        }
    }
}
