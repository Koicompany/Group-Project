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

    private void Fire()
    {
        if (inkLaserPrefab == null)
            return;

        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint not assigned on InkBlast prefab. Using prefab root as fallback.");
        }

        Vector3 spawnPos = (firePoint != null) ? firePoint.position : transform.position;
        Quaternion spawnRot = (firePoint != null) ? firePoint.rotation : transform.rotation;

        InkLaser laser = Instantiate(
            inkLaserPrefab,
            spawnPos,
            spawnRot
        );

        laser.Initialize(targetTag);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
