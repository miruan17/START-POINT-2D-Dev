using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Buttons : MonoBehaviour
{
    // 0~4 상태 정의
    public enum GameState
    {
        Title = 0,            // 타이틀 메인 버튼 보임
        TitleOptions = 1,     // 타이틀 옵션 패널 열림
        Gameplay = 2,         // 인게임 진행 중(메뉴 닫힘)
        PauseMenu = 3,        // 인게임 메뉴(ESC) 열림
        PauseMenuOption = 4   // 인게임 메뉴 상태에서 옵션 패널 열림
    }

    [Header("효과음 나는 버튼")]
    public GameObject[] soundButtons;      // 클릭 시 효과음 붙일 버튼들
    public AudioSource sfxSource;
    public AudioClip clickClip;

    [Header("타이틀 화면")]
    public GameObject[] mainButtons;       // Title에서 보이는 메인 버튼 묶음
    public GameObject optionPanel;         // TitleOptions에서 보이는 옵션 패널

    [Header("인게임 메뉴")]
    public GameObject optionIngame;        // PauseMenu에서 보이는 인게임 메뉴 루트
    public GameObject optionPanelIngame;   // PauseMenuOption에서 보이는 인게임 옵션 패널(※ 부모가 꺼져있으면 보이지 않음)

    [Header("상태/옵션")]
    [SerializeField] private GameState state = GameState.Title;
    [SerializeField] private bool pauseUsesTimeScale = true; // 일시정지 시 Time.timeScale=0

    // (호환용) 외부에서 참조할 수 있게 유지하는 inGame 플래그(0/1)
    [SerializeField, HideInInspector] private int inGame = 0;

    void Start()
    {
        // 클릭 사운드 바인딩
        if (soundButtons != null)
        {
            foreach (var btnObj in soundButtons)
            {
                if (!btnObj) continue;
                var btn = btnObj.GetComponent<Button>();
                if (btn != null) btn.onClick.AddListener(PlayClickSound);
            }
        }

        // 초기 상태 적용
        ApplyState();
    }

    void Update()
    {
        // ESC로 상태 토글(일관성 위해 상태 기반으로 통일)
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
                // Title은 무시
            }
        }
    }

    // ===== 상태 전환(핵심) =====
    public void SetState(GameState newState)
    {
        state = newState;
        ApplyState();
    }

    // 인스펙터에서 int로 연결하고 싶을 때(0~4)
    public void SetInGame(int code)
    {
        if (code < 0) code = 0;
        if (code > 4) code = 4;
        SetState((GameState)code);
    }

    // 네가 정의한 표 그대로 on/off 적용:
    // state	mainButtons	optionPanel	optionIngame	optionPanelIngame
    // 0      on           off          off           off
    // 1      off          on           off           off
    // 2      off          off          off           off
    // 3      off          off          on            off
    // 4      off          off          off           on
    void ApplyState()
    {
        bool showTitleMain      = state == GameState.Title;           // 0
        bool showTitleOption    = state == GameState.TitleOptions;    // 1
        bool showPauseRoot      = state == GameState.PauseMenu;       // 3
        bool showPauseOption    = state == GameState.PauseMenuOption; // 4

        // 타이틀 메인 버튼
        if (mainButtons != null)
            foreach (var go in mainButtons) if (go) go.SetActive(showTitleMain);

        // 타이틀 옵션 패널
        if (optionPanel) optionPanel.SetActive(showTitleOption);

        // 인게임 메뉴 루트
        if (optionIngame) optionIngame.SetActive(showPauseRoot);

        // 인게임 옵션 패널(주의: 부모가 꺼져있으면 보이지 않음)
        if (optionPanelIngame) optionPanelIngame.SetActive(showPauseOption);

        // inGame(0/1) 동기화
        inGame = (state == GameState.Gameplay || state == GameState.PauseMenu || state == GameState.PauseMenuOption) ? 1 : 0;

        // 일시정지 시 TimeScale 제어
        if (pauseUsesTimeScale)
        {
            bool paused = (state == GameState.PauseMenu || state == GameState.PauseMenuOption);
            Time.timeScale = paused ? 0f : 1f;
        }
    }

    // ===== 버튼 핸들러(이름 유지, 내부는 숫자만 변경) =====
    public void NewgameButton()             { SetInGame(2); } // Gameplay
    public void LoadButton()                { SetInGame(2); } // (로드 완료 후) Gameplay

    // 옵션 버튼: Title에서는 TitleOptions, PauseMenu에서는 PauseMenuOption
    public void OptionButton()
    {
        if (state == GameState.Title)            SetInGame(1);
        else if (state == GameState.PauseMenu)   SetInGame(4);
    }

   private void CloseOptionButton()
    {
    if (state == GameState.TitleOptions)       SetInGame(0); // 1 -> 0
    else if (state == GameState.PauseMenuOption) SetInGame(3); // 4 -> 3
    }

    // 인게임 계속하기(일시정지 해제)
    public void ResumeButton()
    {
        if (state == GameState.PauseMenu || state == GameState.PauseMenuOption)
            SetInGame(2);
    }

    // 종료
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
}
