using UnityEngine;
using System.Collections;

public class RockRainUltimate : MonoBehaviour
{
    [Header("Rock Settings")]
    [SerializeField] private FallingRock rockPrefab;
    [SerializeField] private float duration = 3f;
    [SerializeField] private float spawnRate = 0.25f;
    [SerializeField] private float damage = 10f;

    [Header("Spawn Area")]
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private float spawnY = 8f;

    private string enemyTag;

    // Called by UltimateMoveManager
    public void Initialize(string enemyTag)
    {
        this.enemyTag = enemyTag;
        StartCoroutine(SpawnRoutine());
        Debug.Log("Rock Rain Ultimate Activated");
    }

    private IEnumerator SpawnRoutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            SpawnRock();
            yield return new WaitForSeconds(spawnRate);
            elapsed += spawnRate;
        }

        Destroy(gameObject, 1f);
    }

    private void SpawnRock()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(minX, maxX),
            spawnY,
            0f
        );

        FallingRock rock = Instantiate(rockPrefab, spawnPos, Quaternion.identity);
        rock.Initialize(enemyTag, damage);
    }
}
