using UnityEngine;

[ExecuteAlways]
public class Hitbox : MonoBehaviour
{
    public LayerMask targetLayer;
    private BoxCollider2D boxCollider;
    private void OnDrawGizmos()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
    }

    
}
