using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class ScissorsDashAbility : MonoBehaviour
{
    [Header("Dash Settings")]
    public KeyCode dashKey = KeyCode.R;
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.4f;

    [Header("Extra Delay Before Dash (ms)")]
    public float dashDelayBeforeMove = 0.05f;  // << NEW

    [Header("Damage Settings")]
    public int dashDamage = 15;
    public float hitboxRadius = 0.7f;
    public LayerMask damageTargets;

    [Header("Pass-Through Layers")]
    public LayerMask passThroughLayers;

    [Header("Layers (player's normal layer name)")]
    public string playerLayerName = "Player";

    private Rigidbody2D rb;
    private Animator anim;

    private WASD wasdMovement;
    private Arrows arrowsMovement;
    private SizeRestraint sizeRestraint;

    private bool isDashing = false;
    private bool onCooldown = false;
    private bool dashDelayActive = false;

    private float dashTimer;
    private float cooldownTimer;
    private float dashDelayTimer;

    private int dashDirection = 1;

    private HashSet<Collider2D> alreadyHit = new HashSet<Collider2D>();
    private List<int> ignoredLayerIndices = new List<int>();
    private int playerLayer = -1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        wasdMovement = GetComponent<WASD>();
        arrowsMovement = GetComponent<Arrows>();
        sizeRestraint = GetComponentInChildren<SizeRestraint>();

        playerLayer = LayerMask.NameToLayer(playerLayerName);
        if (playerLayer == -1)
            Debug.LogWarning($"[Dash] Player layer '{playerLayerName}' not found.");
    }

    private void Update()
    {
        HandleInput();
        HandleTimers();
    }

    private void HandleInput()
    {
        if (isDashing || onCooldown || dashDelayActive) return;

        if (Input.GetKeyDown(dashKey))
        {
            float horizInput = 0f;
            if (wasdMovement) horizInput = wasdMovement.horizontalInput;
            else if (arrowsMovement) horizInput = arrowsMovement.horizontalInput;

            if (Mathf.Abs(horizInput) > 0.01f)
                dashDirection = horizInput > 0 ? 1 : -1;
            else if (sizeRestraint)
                dashDirection = sizeRestraint.transform.localScale.x > 0 ? -1 : 1;
            else
                dashDirection = transform.localScale.x > 0 ? 1 : -1;

            BeginDashDelay();
        }
    }

    private void BeginDashDelay()
    {
        dashDelayActive = true;
        dashDelayTimer = dashDelayBeforeMove;

        if (anim) anim.SetTrigger("tornado");

        if (wasdMovement) wasdMovement.blockJumping = true;
        if (arrowsMovement) arrowsMovement.blockJumping = true;

        onCooldown = true;
        cooldownTimer = dashCooldown;

        alreadyHit.Clear();
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;

        // Enable pass-through
        if (playerLayer != -1)
        {
            ignoredLayerIndices.Clear();
            for (int layer = 0; layer < 32; layer++)
            {
                if (((1 << layer) & passThroughLayers) != 0)
                {
                    Physics2D.IgnoreLayerCollision(playerLayer, layer, true);
                    ignoredLayerIndices.Add(layer);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing) return;

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);

        DealDamage();
    }

    private void DealDamage()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, hitboxRadius, damageTargets);

        foreach (var col in hits)
        {
            if (col == null) continue;
            if (alreadyHit.Contains(col)) continue;

            alreadyHit.Add(col);

            var h = col.GetComponent<Health>();
            if (h != null)
                h.TakeDamage(dashDamage); // << no knockback version
        }
    }

    private void HandleTimers()
    {
        if (dashDelayActive)
        {
            dashDelayTimer -= Time.deltaTime;
            if (dashDelayTimer <= 0f)
            {
                dashDelayActive = false;
                StartDash();
            }
        }

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

        if (wasdMovement) wasdMovement.blockJumping = false;
        if (arrowsMovement) arrowsMovement.blockJumping = false;

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        foreach (int layer in ignoredLayerIndices)
            Physics2D.IgnoreLayerCollision(playerLayer, layer, false);

        ignoredLayerIndices.Clear();
        alreadyHit.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitboxRadius);
    }
}
