using UnityEngine;
using System.Collections;

public class InkBlast : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InkLaser inkLaserPrefab;
    [SerializeField] private float duration = 2.5f;

    private string targetTag;
    private Transform firePoint;
    private UltimateMoveManager owner;

    public void Initialize(
        UltimateMoveManager owner,
        Transform firePoint,
        string targetTag)
    {
        this.owner = owner;
        this.firePoint = firePoint;
        this.targetTag = targetTag;

        Fire();
        StartCoroutine(Lifetime());
    }

    private void Fire()
    {
        InkLaser laser = Instantiate(
            inkLaserPrefab,
            firePoint.position,
            firePoint.rotation,
            transform
        );

        laser.Initialize(targetTag);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
