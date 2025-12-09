using UnityEngine;

// Run after default so our FixedUpdate can overwrite velocities set by movement scripts
[DefaultExecutionOrder(1000)]
public class ScissorsDashAbility : MonoBehaviour
{
    [Header("Dash Settings")]
    public KeyCode dashKey = KeyCode.R;     // Set to N for Player 2 prefab
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.4f;

    [Header("Debug")]
    public bool debugLogs = false;

    private Rigidbody2D rb;
    private Animator anim;

    private WASD wasdMovement;
    private Arrows arrowsMovement;
    private SizeRestraint sizeRestraint;

    private bool isDashing = false;
    private bool onCooldown = false;

    private float dashTimer = 0f;
    private float cooldownTimer = 0f;

    // cached dash direction (1 or -1)
    private int dashDirection = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        wasdMovement = GetComponent<WASD>();
        arrowsMovement = GetComponent<Arrows>();
        sizeRestraint = GetComponentInChildren<SizeRestraint>();

        if (rb == null)
            Debug.LogError("[ScissorsDashAbility] Rigidbody2D missing on " + gameObject.name);
    }

    private void Update()
    {
        HandleDashInput();
        HandleTimers();
    }

    private void HandleDashInput()
    {
        if (isDashing || onCooldown) return;

        if (Input.GetKeyDown(dashKey))
        {
            // Determine direction preference:
            // 1) if movement input exists (player actively pushing left/right) use that
            // 2) otherwise fallback to SizeRestraint facing, inverted to match how your sprite flips
            float horizInput = 0f;
            if (wasdMovement != null) horizInput = wasdMovement.horizontalInput;
            else if (arrowsMovement != null) horizInput = arrowsMovement.horizontalInput;

            if (Mathf.Abs(horizInput) > 0.01f)
            {
                dashDirection = horizInput > 0 ? 1 : -1;
                if (debugLogs) Debug.Log("[Dash] using input direction: " + dashDirection);
            }
            else if (sizeRestraint != null)
            {
                // You previously needed an inversion for SizeRestraint -> keep that fix:
                float sx = sizeRestraint.transform.localScale.x;
                dashDirection = (sx > 0f) ? -1 : 1;
                if (debugLogs) Debug.Log("[Dash] using SizeRestraint facing (inverted): " + dashDirection + " (scale.x=" + sx + ")");
            }
            else
            {
                float sx = transform.localScale.x;
                dashDirection = (sx > 0f) ? 1 : -1; // fallback: normal scale mapping
                if (debugLogs) Debug.Log("[Dash] using fallback transform scale: " + dashDirection);
            }

            StartDash();
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;

        // Play dash animation if present (no error if missing)
        if (anim) anim.SetTrigger("dash");

        // Optionally block jumping during dash so players can't spam jumps
        if (wasdMovement != null) wasdMovement.blockJumping = true;
        if (arrowsMovement != null) arrowsMovement.blockJumping = true;

        // Start cooldown immediately
        onCooldown = true;
        cooldownTimer = dashCooldown;

        if (debugLogs) Debug.Log($"[Dash] Start (dir={dashDirection}, speed={dashSpeed}) on {gameObject.name}");
        // We apply actual velocity in FixedUpdate so it survives movement script writes.
    }

    // Apply physics velocity here so it runs after player's movement scripts (which run earlier in FixedUpdate)
    private void FixedUpdate()
    {
        if (!isDashing) return;

        // Preserve vertical velocity so mid-air dash works
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);
    }

    private void HandleTimers()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
                EndDash();
        }

        if (onCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                onCooldown = false;
        }
    }

    private void EndDash()
    {
        isDashing = false;

        // Re-enable jumping
        if (wasdMovement != null) wasdMovement.blockJumping = false;
        if (arrowsMovement != null) arrowsMovement.blockJumping = false;

        // stop horizontal momentum so player doesn't keep sliding
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        if (debugLogs) Debug.Log("[Dash] End");
    }
}
