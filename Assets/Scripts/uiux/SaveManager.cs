using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public interface ISaveable { object CaptureState(); void RestoreState(object state); string SaveKey { get; } }

[System.Serializable] public class SaveKV{ public string key; public string json; }
[System.Serializable] public class SaveSlot{ public int stageId; public float[] playerPos; public string savedAt; }
[System.Serializable] public class SaveData{ public List<SaveKV> states=new(); public SaveSlot meta=new(); }

public class SaveManager : MonoBehaviour
{
    public static SaveManager I { get; private set; }
    readonly List<ISaveable> saveables = new();
    float lastSaveAt; public float saveCooldown=1f;
    string SavePath => Path.Combine(Application.persistentDataPath, "autosave.json");

    void Awake(){
        if (I!=null){ Destroy(gameObject); return; }
        I=this; DontDestroyOnLoad(gameObject);
        Application.quitting += () => AutoSave("quitting");
    }

    public void Register(ISaveable s){ if (!saveables.Contains(s)) saveables.Add(s); }
    public void Unregister(ISaveable s){ saveables.Remove(s); }

    public void AutoSave(string reason="manual", int currentStageId=-1, Vector3? playerPos=null)
    {
        if (Time.unscaledTime - lastSaveAt < saveCooldown) return;
        lastSaveAt = Time.unscaledTime;

        var data = new SaveData();
        data.meta.stageId=currentStageId;
        data.meta.savedAt=System.DateTime.UtcNow.ToString("o");
        if (playerPos.HasValue){ var p=playerPos.Value; data.meta.playerPos=new float[]{p.x,p.y,p.z}; }

        foreach (var s in saveables){
            var st = s.CaptureState(); if (st==null) continue;
            data.states.Add(new SaveKV{ key=s.SaveKey, json=JsonUtility.ToJson(st) });
        }

        File.WriteAllText(SavePath, JsonUtility.ToJson(data,true), Encoding.UTF8);
    #if UNITY_EDITOR
        Debug.Log($"[SaveManager] AutoSaved ({reason}) -> {SavePath}");
    #endif
    }

    public bool TryLoad(out SaveData data){
        data=null; if (!File.Exists(SavePath)) return false;
        data = JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath, Encoding.UTF8));
        return data!=null;
    }

    public void RestoreAll(SaveData data)
    {
        foreach (var s in saveables){
            var kv = data.states.Find(x=>x.key==s.SaveKey);
            if (kv==null || string.IsNullOrEmpty(kv.json)) continue;
            var sample = s.CaptureState(); if (sample==null) continue;
            var stateObj = JsonUtility.FromJson(kv.json, sample.GetType());
            s.RestoreState(stateObj);
        }
    }
}
