using UnityEngine;

public class UltimateMoveManager : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private string enemyTag; // "Player1" or "Player2"

    [Header("Ultimate Moves")]
    [SerializeField] private InkBlast inkBlastPrefab;
    [SerializeField] private RockRainUltimate rockRainPrefab;

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
<<<<<<< HEAD
<<<<<<< HEAD


=======
=======
<<<<<<< Updated upstream
>>>>>>> 19d8fdbb21198f168fff7bc7dc3055026edc5c6b
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
<<<<<<< HEAD
>>>>>>> 4be101a5df99dcae8028d51143032bf196e739de
=======
=======


>>>>>>> Stashed changes
>>>>>>> 19d8fdbb21198f168fff7bc7dc3055026edc5c6b
}

