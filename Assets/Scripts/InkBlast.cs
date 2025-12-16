using UnityEngine;
using System.Collections;

public class InkBlast : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InkLaser inkLaserPrefab;
    [SerializeField] private float duration = 4f;

    [Header("Spawn Point")]
    [SerializeField] private Transform firePoint; // <-- drag FirePoint here in inspector

    private string targetTag;

    // Initialize with just the target tag
    public void Initialize(string targetTag)
    {
        this.targetTag = targetTag;

        Fire();
        StartCoroutine(Lifetime());
    }

    private InkLaser currentLaser;

    private void Fire()
    {
        if (inkLaserPrefab == null) return;

        Vector3 spawnPos = (firePoint != null) ? firePoint.position : transform.position;

        // Mirror the rotation if player is flipped
        Quaternion spawnRot = Quaternion.identity;
        if (firePoint != null)
            spawnRot = firePoint.rotation;

        // Check if the parent is flipped
        if (transform.localScale.x < 0)
            spawnRot *= Quaternion.Euler(0, 180, 0); // flip horizontally

        currentLaser = Instantiate(
            inkLaserPrefab,
            spawnPos,
            spawnRot
        );

        currentLaser.Initialize(targetTag);
    }


    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(duration);

        if (currentLaser != null)
            Destroy(currentLaser.gameObject); // destroy the spawned laser

        Destroy(gameObject); // optional: destroy the InkBlast object itself if needed
    }

}
