using UnityEngine;

public class PlayerSave : MonoBehaviour, ISaveable
{
    [System.Serializable]
    private class PlayerState { public float x, y, z; }

    public string SaveKey => "Player";

    void OnEnable()  { SaveManager.I?.Register(this); }
    void OnDisable() { SaveManager.I?.Unregister(this); }

    public object CaptureState()
    {
        var p = transform.position;
        return new PlayerState { x = p.x, y = p.y, z = p.z };
    }

    public void RestoreState(object state)
    {
        var s = (PlayerState)state;
        transform.position = new Vector3(s.x, s.y, s.z);
    }
}
