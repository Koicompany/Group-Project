using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ChargeMeter : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private string targetPlayerTag = "Player1";

    [Header("UI")]
    [SerializeField] private Image chargeFill;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Charge Values")]
    [SerializeField] private float maxCharge = 100f;
    [SerializeField] private float damageTakenGain = 10f;
    [SerializeField] private float damageDealtGain = 15f;
    [SerializeField] private float fillSpeed = 3f;

    [Header("AR Trigger")]
    [SerializeField] private GameObject arCameraObject;
    [SerializeField] private GameObject arDisplayObject;
    [SerializeField] private float arActiveTime = 3f;

    private float currentCharge;
    private float displayedCharge;
    private bool arTriggered;
    private Health playerHealth;

    private void Start()
    {
        currentCharge = 0f;
        displayedCharge = 0f;

        chargeFill.fillAmount = 0f;

        // Timer text is ALWAYS active — just clear it
        if (timerText != null)
            timerText.text = "";

        if (arCameraObject != null)
            arCameraObject.SetActive(false);

        if (arDisplayObject != null)
            arDisplayObject.SetActive(false);

        InvokeRepeating(nameof(TryFindPlayer), 0f, 0.25f);
        Health.OnAnyPlayerDamaged += HandleGlobalDamage;
    }

    private void OnDestroy()
    {
        Health.OnAnyPlayerDamaged -= HandleGlobalDamage;

        if (playerHealth != null)
            playerHealth.OnDamageTaken -= HandleDamageTaken;
    }

    private void TryFindPlayer()
    {
        if (playerHealth != null) return;

        GameObject player = GameObject.FindWithTag(targetPlayerTag);
        if (player == null) return;

        playerHealth = player.GetComponent<Health>();
        if (playerHealth == null) return;

        playerHealth.OnDamageTaken += HandleDamageTaken;
        CancelInvoke(nameof(TryFindPlayer));
    }

    private void Update()
    {
        displayedCharge = Mathf.Lerp(
            displayedCharge,
            currentCharge,
            Time.deltaTime * fillSpeed
        );

        chargeFill.fillAmount = displayedCharge / maxCharge;
    }

    private void HandleDamageTaken(float damage)
    {
        AddCharge(damageTakenGain);
    }

    private void HandleGlobalDamage(string damagedTag, float damage)
    {
        if (damagedTag != targetPlayerTag)
            AddCharge(damageDealtGain);
    }

    private void AddCharge(float amount)
    {
        currentCharge = Mathf.Clamp(currentCharge + amount, 0f, maxCharge);

        if (!arTriggered && currentCharge >= maxCharge)
        {
            arTriggered = true;
            StartCoroutine(ActivateARCameraWithTimer());
        }
    }

    private IEnumerator ActivateARCameraWithTimer()
    {
        if (arCameraObject != null)
            arCameraObject.SetActive(true);

        if (arDisplayObject != null)
            arDisplayObject.SetActive(true);

        float remainingTime = arActiveTime;

        while (remainingTime > 0f)
        {
            if (timerText != null)
                timerText.text = remainingTime.ToString("F1");

            remainingTime -= Time.deltaTime;
            yield return null;
        }

        // Cleanup
        if (timerText != null)
            timerText.text = "";

        if (arCameraObject != null)
            arCameraObject.SetActive(false);

        if (arDisplayObject != null)
            arDisplayObject.SetActive(false);

        currentCharge = 0f;
        arTriggered = false;
    }
}
