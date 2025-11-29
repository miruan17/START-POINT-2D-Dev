using UnityEngine;

public class VFXFollow : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if (target == null) return;

        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
