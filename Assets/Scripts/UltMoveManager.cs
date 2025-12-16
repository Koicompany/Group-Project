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

    // CALLED BY VUFORIA
    public void ActivateInkBlast()
    {
        if (inkBlastPrefab == null)
            return;

        // No need to pass FirePoint anymore
        InkBlast blast = Instantiate(
            inkBlastPrefab,
            transform.position,
            transform.rotation
        );

        blast.Initialize(enemyTag); // only pass target tag
    }


}
