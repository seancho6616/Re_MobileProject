using UnityEngine;
using TMPro; 
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject signinPanel;

    [Header("Popup UI")]
    public GameObject noticePopup;       // 팝업창 전체 오브젝트
    public TextMeshProUGUI noticeText;   // 팝업창 텍스트
    public Button noticeCloseButton;     // 팝업 닫기 버튼

    [Header("Login UI")]
    public TMP_InputField loginIdInput;
    public TMP_InputField loginPwInput; 
    public Button loginButton;
    public Button goToRegisterButton;

    [Header("Register UI")]
    public TMP_InputField regIdInput;   
    public TMP_InputField regPwInput;
    public TMP_InputField regNickInput;
    public Button registerSubmitButton;

    void Start()
    {
        // 초기 화면 설정
        loginPanel.SetActive(true);
        signinPanel.SetActive(false);
        
        // 시작할 때 팝업 꺼두기
        if(noticePopup != null) noticePopup.SetActive(false);

        // 버튼 리스너 연결
        loginButton.onClick.AddListener(OnLoginClick);
        goToRegisterButton.onClick.AddListener(ShowRegisterPanel);
        registerSubmitButton.onClick.AddListener(OnRegisterSubmitClick);
        
        if(noticeCloseButton != null) noticeCloseButton.onClick.AddListener(ClosePopup);
    }

    void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        signinPanel.SetActive(true);
        // 입력 필드 초기화
        regIdInput.text = "";
        regPwInput.text = "";
        regNickInput.text = "";
    }

    void ShowLoginPanel()
    {
        signinPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    // --- 팝업 함수 ---
    void ShowPopup(string message)
    {
        if (noticePopup != null && noticeText != null)
        {
            noticeText.text = message; 
            noticePopup.SetActive(true); 
        }
        else
        {
            // 팝업 UI가 연결 안 되어 있을 때를 대비해 로그도 출력
            Debug.Log("알림: " + message);
        }
    }

    void ClosePopup()
    {
        if (noticePopup != null) noticePopup.SetActive(false);
    }

    // --- 로그인 요청 ---
    void OnLoginClick()
    {
        Sound.instance.PlayClick();
        string id = loginIdInput.text;
        string pw = loginPwInput.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            ShowPopup("아이디와 비밀번호를 입력하세요.");
            return;
        }

        // [수정 완료] (isSuccess, errorMsg) 2개의 인자를 받도록 처리
        StartCoroutine(NetworkManager.Instance.Login(id, pw, (isSuccess, errorMsg) =>
        {
            if (isSuccess)
            {
                Debug.Log("로그인 성공");
                SceneManager.LoadScene("StartScene"); 
            }
            else
            {
                // 실패 시 에러 메시지 팝업
                // 서버에서 메시지가 안 왔을 경우를 대비한 기본 문구
                if (string.IsNullOrEmpty(errorMsg)) errorMsg = "아이디 또는 비밀번호가 틀렸습니다.";
                ShowPopup(errorMsg);
            }
        }));
    }

    // --- 회원가입 요청 ---
    void OnRegisterSubmitClick()
    {
        Sound.instance.PlayClick();
        string id = regIdInput.text;
        string pw = regPwInput.text;
        string nick = regNickInput.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw) || string.IsNullOrEmpty(nick))
        {
            ShowPopup("모든 정보를 입력해주세요.");
            return;
        }

        // [수정 완료] (isSuccess, errorMsg) 2개의 인자를 받도록 처리
        StartCoroutine(NetworkManager.Instance.Register(id, pw, nick, (isSuccess, errorMsg) =>
        {
            if (isSuccess)
            {
                ShowPopup("가입 성공! 로그인 해주세요.");
                ShowLoginPanel(); 
            }
            else
            {
                // "중복" 에러 처리 (서버 메시지 또는 코드 11000 체크)
                if (errorMsg.Contains("중복") || errorMsg.Contains("사용 중") || errorMsg.Contains("11000"))
                {
                    ShowPopup("이미 존재하는 아이디입니다.");
                }
                else
                {
                    ShowPopup(errorMsg);
                }
            }
        }));
    }
}