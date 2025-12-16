using UnityEngine;
using System.Collections;

public class InkBlast : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InkLaser inkLaserPrefab;
    [SerializeField] private float duration = 4f;

    [Header("Spawn Point")]
    [SerializeField] private Transform firePoint; // assign in prefab

    private string targetTag;
    private InkLaser currentLaser;

    public void Initialize(string targetTag, bool facingRight)
    {
        this.targetTag = targetTag;

        ApplyFacing(facingRight);
        Fire();
        StartCoroutine(Lifetime());
    }

    private void ApplyFacing(bool facingRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void Fire()
    {
        if (inkLaserPrefab == null || firePoint == null)
        {
            Debug.LogError("InkBlast missing InkLaser prefab or FirePoint reference.");
            return;
        }

        currentLaser = Instantiate(
            inkLaserPrefab,
            firePoint.position,
            firePoint.rotation,
            transform   // parent to InkBlast
        );

        currentLaser.Initialize(targetTag);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(duration);

        if (currentLaser != null)
            Destroy(currentLaser.gameObject);

        Destroy(gameObject);
    }
}
