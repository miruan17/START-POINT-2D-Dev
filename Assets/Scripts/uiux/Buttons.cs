using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Buttons : MonoBehaviour
{
    
    public enum GameState
    {
        Title = 0,           
        TitleOptions = 1,   
        Gameplay = 2,       
        PauseMenu = 3,        
        PauseMenuOption = 4   
    }

    [Header("효과음 나는 버튼")]
    public GameObject[] soundButtons;    
    public AudioSource sfxSource;
    public AudioClip clickClip;

    [Header("타이틀 화면 버튼")]
    public GameObject[] mainButtons;       
    public GameObject optionPanel;       

    [Header("인게임 메뉴")]
    public GameObject optionIngame;       

    [Header("상태/옵션")]
    [SerializeField] private GameState state = GameState.Title;
    [SerializeField] private bool pauseUsesTimeScale = true; 

    
    [SerializeField, HideInInspector] private int inGame = 0;

    void Start()
    {
        
        if (soundButtons != null)
        {
            foreach (var btnObj in soundButtons)
            {
                if (!btnObj) continue;
                var btn = btnObj.GetComponent<Button>();
                if (btn != null) btn.onClick.AddListener(PlayClickSound);
            }
        }

        
        ApplyState();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (state)
            {
                case GameState.Gameplay:
                    SetState(GameState.PauseMenu);
                    break;
                case GameState.PauseMenu:
                    SetState(GameState.Gameplay);
                    break;
                case GameState.PauseMenuOption:
                    SetState(GameState.PauseMenu);
                    break;
                case GameState.TitleOptions:
                    SetState(GameState.Title);
                    break;
                    
            }
        }
    }

    
    public void SetState(GameState newState)
    {
        state = newState;
        ApplyState();
    }

   
    public void SetInGame(int code)
    {
        if (code < 0) code = 0;
        if (code > 4) code = 4;
        SetState((GameState)code);
    }

    
    // state	mainButtons	optionPanel	optionIngame	optionPanelIngame
    // 0      on           off          off           off
    // 1      off          on           off           off
    // 2      off          off          off           off
    // 3      off          off          on            off
    // 4      off          off          off           on
    void ApplyState()
    {
        bool isTitleMain = state == GameState.Title;
        bool isTitleOpt = state == GameState.TitleOptions;
        bool isPauseRoot = state == GameState.PauseMenu;
        bool isPauseOpt = state == GameState.PauseMenuOption;

        if (mainButtons != null)
            foreach (var go in mainButtons) if (go) go.SetActive(isTitleMain);

        if (optionPanel) optionPanel.SetActive(isTitleOpt||isPauseOpt);

        if (optionIngame) optionIngame.SetActive(isPauseRoot || isPauseOpt);

        inGame = (state == GameState.Gameplay || isPauseRoot || isPauseOpt) ? 1 : 0;

        if (pauseUsesTimeScale)
        {
            bool paused = (isPauseRoot || isPauseOpt);
            Time.timeScale = paused ? 0f : 1f;
        }
    }

    public void NewgameButton() { SetInGame(2); } // 
    public void LoadButton() { SetInGame(2); } // 

   
    public void OptionButton()
    {
        if (state == GameState.Title)
        { SetInGame(1); }
        else if (state == GameState.PauseMenu)
        { SetInGame(4); }
    }

    public void CloseOptionButton()
    {
        if (state == GameState.TitleOptions)
        { SetInGame(0); }
        else if (state == GameState.PauseMenuOption)
        { SetInGame(3); }
    }

    public void ResumeButton()
    {
        if (state == GameState.PauseMenu || state == GameState.PauseMenuOption)
            SetInGame(2);
    }

    public void TitleButton()
    {
        SetInGame(0);
    }

    
    public void ExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    void PlayClickSound()
    {
        if (sfxSource && clickClip) sfxSource.PlayOneShot(clickClip);
    }
    
    void OnDisable()
{
    if (pauseUsesTimeScale) Time.timeScale = 1f;
}
void OnDestroy()
{
    if (pauseUsesTimeScale) Time.timeScale = 1f;
}

}
