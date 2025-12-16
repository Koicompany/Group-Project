using UnityEngine;
using System.Collections;

public class SpecialAttackController : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private string enemyTag;

    [Header("Boulder Attack")]
    [SerializeField] private GameObject boulderPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float attackDuration = 3f;
    [SerializeField] private float spawnInterval = 0.3f;
    [SerializeField] private float fallSpeed = 8f;

    private bool attacking = false;

    public void TriggerSpecial()
    {
        if (attacking) return;
        StartCoroutine(BoulderRain());
    }

    private IEnumerator BoulderRain()
    {
        attacking = true;

        float timer = 0f;

        while (timer < attackDuration)
        {
            SpawnBoulder();
            timer += spawnInterval;
            yield return new WaitForSeconds(spawnInterval);
        }

        attacking = false;
    }

    private void SpawnBoulder()
    {
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject boulder = Instantiate(boulderPrefab, spawn.position, Quaternion.identity);

        Rigidbody2D rb = boulder.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.down * fallSpeed;
        }

        BoulderDamage damage = boulder.GetComponent<BoulderDamage>();
        if (damage != null)
        {
            damage.Init(enemyTag);
        }
    }
}
