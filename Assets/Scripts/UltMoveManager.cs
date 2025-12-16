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



}
