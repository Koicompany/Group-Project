using UnityEngine;

public class UltimateMoveManager : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private string enemyTag; // "Player1" or "Player2"

    [Header("Ultimate Moves")]
    [SerializeField] private InkBlast inkBlastPrefab;
    [SerializeField] private RockRainUltimate rockRainPrefab;
    [SerializeField] private ScissorSlashUltimate scissorSlashPrefab;


    private Transform firePoint;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");

    }

    // CALLED BY VUFORIA
    public void ActivateInkBlast()
    {
        if (inkBlastPrefab == null)
            return;

        bool facingRight = transform.localScale.x >= 0f;

        InkBlast blast = Instantiate(
            inkBlastPrefab,
            transform.position,
            Quaternion.identity
        );

        blast.Initialize(enemyTag, facingRight);
    }
    public void ActivateRockRain()
    {
        if (rockRainPrefab == null)
            return;

        RockRainUltimate rain = Instantiate(
            rockRainPrefab,
            Vector3.zero,
            Quaternion.identity
        );

        rain.Initialize(enemyTag);
    }

    public void ActivateScissorSlash()
    {
        if (scissorSlashPrefab == null || firePoint == null)
            return;

        ScissorSlashUltimate slash = Instantiate(
            scissorSlashPrefab,
            firePoint.position,
            firePoint.rotation
        );

        slash.Initialize(enemyTag);
    }

}
