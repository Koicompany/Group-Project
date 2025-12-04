using UnityEngine;

public class SizeRestraint : MonoBehaviour
{
    private Vector3 originalLocalScale;
    private int flipSign = 1; // 1 = normal, -1 = flipped

    private void Awake()
    {
        originalLocalScale = transform.localScale;
    }

    public void Flip(bool facingRight)
    {
        flipSign = facingRight ? 1 : -1;
    }

    private void LateUpdate()
    {
        Vector3 parentScale = transform.parent ? transform.parent.lossyScale : Vector3.one;

        // Apply flipping relative to original local scale and parent's scale
        transform.localScale = new Vector3(
            originalLocalScale.x * flipSign / parentScale.x,
            originalLocalScale.y / parentScale.y,
            originalLocalScale.z / parentScale.z
        );
    }
}
