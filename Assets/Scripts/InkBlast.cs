using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InkBlast : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] private InkLaser laserPrefab;
    [SerializeField] private float blastDuration = 2.5f;

    [Header("Targeting")]
    [SerializeField] private string firePointName = "FirePoint";

    private bool isFiring;

    // CALLED BY VUFORIA
    public void Fire(string targetTag)
    {
        Debug.Log("[InkBlast] Fire() called");

        if (isFiring)
            return;

        if (laserPrefab == null)
        {
            Debug.LogError("[InkBlast] Laser prefab missing");
            return;
        }

        List<Transform> firePoints = ResolveFirePoints(targetTag);

        if (firePoints.Count == 0)
        {
            Debug.LogWarning("[InkBlast] No fire points found");
            return;
        }

        StartCoroutine(FireRoutine(firePoints, targetTag));
    }

    // FIND FIRE POINTS SAFELY
    private List<Transform> ResolveFirePoints(string targetTag)
    {
        List<Transform> results = new List<Transform>();

        GameObject[] players = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject player in players)
        {
            Transform fp = player.transform.Find(firePointName);

            if (fp != null && fp.gameObject.activeInHierarchy)
            {
                results.Add(fp);
            }
        }

        return results;
    }

    // FIRING LOGIC
    private IEnumerator FireRoutine(List<Transform> firePoints, string targetTag)
    {
        isFiring = true;

        foreach (Transform firePoint in firePoints)
        {
            if (firePoint == null) continue;

            InkLaser laser = Instantiate(
                laserPrefab,
                firePoint.position,
                firePoint.rotation
            );

            laser.Begin(targetTag);
        }

        yield return new WaitForSeconds(blastDuration);

        isFiring = false;
    }
}
