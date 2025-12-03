using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class StartSceneManager : MonoBehaviour
{
    [Header("Main UI")]
    public Button continueButton;   // 이어하기 버튼

    [Header("Popups")]
    public GameObject warningPopup; // 경고창
    public GameObject settingPanel; // 설정창 (여기에 텍스트 다 넣을 예정)

    private bool canContinue = false; 

    void Start()
    {
        // 시작할 때 팝업 끄기
        if (warningPopup != null) warningPopup.SetActive(false);
        if (settingPanel != null) settingPanel.SetActive(false);
        
        if (continueButton != null) continueButton.interactable = true;

        // 서버 데이터 조회
        StartCoroutine(NetworkManager.Instance.LoadGameData((data) =>
        {
            if (data != null)
            {
                if (data.position == null) data.position = new PositionData(0, 0, 0);

                if (GameDataStore.Instance == null)
                {
                    GameObject go = new GameObject("GameDataStore");
                    go.AddComponent<GameDataStore>();
                }
                GameDataStore.Instance.cachedData = data;

                if (data.hasPlayed == false) canContinue = false; 
                else canContinue = true;
            }
            else canContinue = false; 
        }));
    }

    // --- 메인 버튼 ---
    public void OnClickContinue()
    {
        Sound.instance.PlayClick();
        if (canContinue == false)
        {
            if (warningPopup != null) warningPopup.SetActive(true);
            return; 
        }
        GameDataStore.Instance.isContinue = true;
        string sceneToLoad = GameDataStore.Instance.cachedData.lastScene;
        if (string.IsNullOrEmpty(sceneToLoad) || sceneToLoad == "StartArea") sceneToLoad = "MainScene";
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnClickNewGame()
    {
        Sound.instance.PlayClick();
        GameDataStore.Instance.isContinue = false;
        GameDataStore.Instance.cachedData = null;
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickClosePopup()
    {
        Sound.instance.PlayClick();
        if (warningPopup != null) warningPopup.SetActive(false);
    }

    // --- 설정창 기능 ---

    // 1. 설정창 열기
    public void OnClickOpenSettings()
    {
        Sound.instance.PlayClick();
        if (settingPanel != null) settingPanel.SetActive(true);
    }

    // 2. 설정창 닫기 (X 버튼)
    public void OnClickCloseSettings()
    {
        Sound.instance.PlayClick();
        if (settingPanel != null) settingPanel.SetActive(false);
    }

    // 3. 로그아웃 (왼쪽 위 버튼)
    public void OnClickLogout()
    {
        Sound.instance.PlayClick();
        Debug.Log("로그아웃");

        if (NetworkManager.Instance != null) NetworkManager.Instance.authToken = null;
        if (GameDataStore.Instance != null) GameDataStore.Instance.cachedData = null;

        SceneManager.LoadScene("LoginScene");
    }
}