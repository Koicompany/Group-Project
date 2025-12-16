using UnityEngine;

public class UltimateMoveManager : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private string enemyTag; // "Player1" or "Player2"

    [Header("Ultimate Moves")]
    [SerializeField] private InkBlast inkBlastPrefab;

    private Transform firePoint;

    private void Awake()
    {
        // Cache FirePoint safely
        firePoint = transform.Find("FirePoint");

        if (firePoint == null)
            Debug.LogError($"[UltimateMoveManager] FirePoint not found on {name}");
    }

    // =========================
    // CALLED BY VUFORIA
    // =========================
    public void ActivateInkBlast()
    {
        if (inkBlastPrefab == null || firePoint == null)
            return;

        InkBlast blast = Instantiate(
            inkBlastPrefab,
            firePoint.position,
            firePoint.rotation
        );

        blast.Initialize(this, firePoint, enemyTag);
    }
}
