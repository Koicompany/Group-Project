using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InkBlast : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] private InkLaser laserPrefab; // Still uses prefab for the laser object, but no movement
    [SerializeField] private float blastDuration = 2.5f;

    [Header("Fire Points")]
    [SerializeField] private List<Transform> firePoints = new List<Transform>(); // List of fire points

    public void Fire(string targetTag)
    {
        Debug.Log("Firing");
        if (laserPrefab == null || firePoints.Count == 0) return;

        foreach (Transform firePoint in firePoints)
        {
            if (firePoint != null && firePoint.gameObject.activeInHierarchy) // Only active fire points
            {
                // Spawn laser directly at fire point
                InkLaser laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
                laser.Begin(targetTag);
            }
        }

        StartCoroutine(Lifetime());
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(blastDuration);
        Destroy(gameObject);
    }
}
