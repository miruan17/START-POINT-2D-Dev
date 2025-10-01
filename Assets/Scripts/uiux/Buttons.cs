using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Buttons : MonoBehaviour
{
    public enum GameState { Title=0, TitleOptions=1, Gameplay=2, PauseMenu=3, PauseMenuOption=4 }

    [Header("효과음 버튼")]
    public GameObject[] soundButtons;
    public AudioSource sfxSource; public AudioClip clickClip;

    [Header("타이틀 UI")]
    public GameObject[] mainButtons;   // Title 전용 메인 버튼 묶음
    public GameObject optionPanel;     // 옵션 패널(Title/인게임 공용)

    [Header("인게임 UI")]
    public GameObject optionIngame;    // Pause 루트 패널

    [Header("상태")]
    [SerializeField] private GameState state = GameState.Title;
    [SerializeField] private bool pauseUsesTimeScale = true;
    [SerializeField, HideInInspector] private int inGame = 0;

    bool IsTitle => SceneManager.GetActiveScene().name == "Title";
    bool IsInGame => SceneManager.GetActiveScene().name == "InGame";

    void Awake() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDestroy(){ SceneManager.sceneLoaded -= OnSceneLoaded; if (pauseUsesTimeScale) Time.timeScale = 1f; }
    void OnDisable(){ if (pauseUsesTimeScale) Time.timeScale = 1f; }

    void Start()
    {
        if (soundButtons != null)
            foreach (var o in soundButtons){ var b=o?o.GetComponent<Button>():null; if (b!=null) b.onClick.AddListener(PlayClickSound); }
        ApplyState();
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        switch (state)
        {
            case GameState.Gameplay:        SetState(GameState.PauseMenu); break;
            case GameState.PauseMenu:       SetState(GameState.Gameplay);  break;
            case GameState.PauseMenuOption: SetState(GameState.PauseMenu); break;
            case GameState.TitleOptions:    SetState(GameState.Title);     break;
        }
    }

    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        if (s.name=="Title")  SetState(GameState.Title);
        if (s.name=="InGame") SetState(GameState.Gameplay);
    }

    public void SetState(GameState st){ state=st; ApplyState(); }
    public void SetInGame(int code){ if (code<0) code=0; if (code>4) code=4; SetState((GameState)code); }

    // mainButtons | optionPanel | optionIngame
    // Title          on            off          off
    // TitleOptions   off           on           off
    // Gameplay       off           off          off
    // PauseMenu      off           off          on
    // PauseMenuOpt   off           on           on
    void ApplyState()
    {
        bool isTitleMain = state==GameState.Title;
        bool isTitleOpt  = state==GameState.TitleOptions;
        bool isPauseRoot = state==GameState.PauseMenu;
        bool isPauseOpt  = state==GameState.PauseMenuOption;

        if (mainButtons!=null) foreach (var go in mainButtons) if (go) go.SetActive(IsTitle && isTitleMain);
        if (optionPanel)  optionPanel.SetActive(isTitleOpt || isPauseOpt);
        if (optionIngame) optionIngame.SetActive(IsInGame && (isPauseRoot || isPauseOpt));

        inGame = (state==GameState.Gameplay || isPauseRoot || isPauseOpt) ? 1 : 0;

        if (pauseUsesTimeScale)
        {
            bool paused = IsInGame && (isPauseRoot || isPauseOpt);
            Time.timeScale = paused ? 0f : 1f;
        }
    }

    // --- 씬 전환은 SceneFlow 호출 ---
    public void NewgameButton(){ SetInGame(2); SceneFlow.I?.LoadNewGame(); }
    public void LoadButton(){    SetInGame(2); SceneFlow.I?.LoadContinue(); }
    public void TitleButton(){   SceneFlow.I?.ReturnToTitle(); }
    public void ExitButton(){
    #if UNITY_EDITOR
        EditorApplication.isPlaying=false;
    #else
        Application.Quit();
    #endif
    }
    public void ResumeButton()
{
    if (state == GameState.PauseMenu || state == GameState.PauseMenuOption)
        SetInGame(2);  // Gameplay로 복귀 → ApplyState()에서 패널 OFF + Time.timeScale=1
}

    public void OptionButton(){ if (state==GameState.Title) SetInGame(1); else if (state==GameState.PauseMenu) SetInGame(4); }
    public void CloseOptionButton(){ if (state==GameState.TitleOptions) SetInGame(0); else if (state==GameState.PauseMenuOption) SetInGame(3); }

    void PlayClickSound(){ if (sfxSource && clickClip) sfxSource.PlayOneShot(clickClip); }
}
