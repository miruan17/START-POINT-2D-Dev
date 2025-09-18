using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
 
//인스펙터 창에 강제로 띄운다.
[System.Serializable]
public class Sound
{
    public string name; //사운드 이름
 
    public AudioClip clip;  //사운드 파일
    private AudioSource source; //  사운드 플레이어
 
    public float volumn;
    public bool loop;
 
    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }
 
    public void SetVolumn()
    {
        source.volume = volumn;
    }
 
    public void Play()
    {
        source.Play();
    }
 
    public void Stop()
    {
        source.Stop();
    }
 
    public void SetLoop()
    {
        source.loop = true;
    }
 
    public void SetLoopCencel()
    {
        source.loop = false;
    }
 
}
 
 
public class AudioManager : MonoBehaviour
{
    static public AudioManager instance;
 
    [SerializeField]
    public Sound[] sounds;
 
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 : " + i + "=" + sounds[i].name); //추가 될 객체의 이름 
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);    //이 스크립트가 실행되는 객체안에 오디오 객체가 추가 된다. (상하관계)
        }
        
    }
 
    public void Play(string _name)
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            if(_name == sounds[i].name)
            {
                sounds[i].Play();
                return;
            }
        }
    }
 
    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }
 
    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }
 
    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoopCencel();
                return;
            }
        }
    }
 
    public void SetVolumn(string _name, float _Volumn)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].volumn = _Volumn;
                sounds[i].SetVolumn();
                return;
            }
        }
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }
}
 
