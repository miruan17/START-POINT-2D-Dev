using UnityEngine;

public class InGameBootstrapper : MonoBehaviour
{
    [SerializeField] Transform player;

    void Start()
    {
        if (SaveManager.I!=null && SaveManager.I.TryLoad(out var data))
        {
            if (data.meta?.playerPos!=null && data.meta.playerPos.Length==3 && player)
                player.position = new Vector3(data.meta.playerPos[0], data.meta.playerPos[1], data.meta.playerPos[2]);

            SaveManager.I.RestoreAll(data);
        }
    }
}
