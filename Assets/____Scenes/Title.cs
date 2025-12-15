using UnityEngine;

public class Title : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float floatHeight = 0.25f; // How far up/down
    [SerializeField] private float floatSpeed = 1f;     // How fast it moves

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = startPos + Vector3.up * yOffset;
    }
}
