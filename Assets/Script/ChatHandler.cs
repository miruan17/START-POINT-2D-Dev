using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatHandler : MonoBehaviour
{
    public TMP_InputField inputField; // 인풋 필드
    public UnityEngine.UI.Button button; // 버튼
    public RectTransform contentRect; // 텍스트 프리팹 부모로 사용

    public GameObject go;
    private bool activated; // 콘솔창 활성화시 true;

    private void Start()
    {
        activated = false;
        BindButton();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            activated = !activated;
            if (activated)
            {
                go.SetActive(true);
            }
            else if (!activated)
            {
                go.SetActive(false);
            }
        }
    }

    void BindButton()
    {
        //버튼 클릭시 이벤트 추가.
        button.onClick.AddListener(() =>
        {
            //아까 저장해 둔 text 프리팹 갖고 옴. 저장 위치는 다 다를 수 있다
            GameObject textPrefab = Resources.Load<GameObject>("Prefab/ChatText");

            //text 프리팹에서 TMP_Text Component 갖고 온 뒤 inputField 텍스트 대입.
            TMP_Text text = textPrefab.GetComponent<TMP_Text>();
            text.text = inputField.text;

            //Instantiate 하기.
            GameObject instGo = GameObject.Instantiate(textPrefab);

            // 아래 false 옵션 안 주면 Text 프리팹 위치가 이상한 곳에 나온다.
            instGo.transform.SetParent(contentRect, false);


            //작업 끝났으면 inputField text를 초기화.
            inputField.text = "";

        });
    }
}
