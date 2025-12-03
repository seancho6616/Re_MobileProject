using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    [Header("Data Cache")]
    public GameData cachedData;
    public bool isContinue = false;

    [Header("Network Settings")]
    private string baseUrl = "https://throatier-paulita-unprevalent.ngrok-free.dev/api";
    public string authToken;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    // --- 1. 회원가입 및 로그인 (Action<bool, string> 사용) ---

    public IEnumerator Register(string username, string password, string nickname, Action<bool, string> onResult)
    {
        var data = new UserRegisterData { username = username, password = password, nickname = nickname };

        yield return SendRequest("/register", "POST", data,
            (json) => {
                Debug.Log("회원가입 성공");
                onResult?.Invoke(true, "");
            },
            (errorMsg) => {
                onResult?.Invoke(false, errorMsg);
            });
    }

    public IEnumerator Login(string username, string password, Action<bool, string> onResult)
    {
        var data = new UserLoginData { username = username, password = password };

        yield return SendRequest("/login", "POST", data,
            (json) => {
                var res = JsonUtility.FromJson<LoginResponse>(json);
                this.authToken = res.token;
                Debug.Log("로그인 성공");
                onResult?.Invoke(true, "");
            },
            (errorMsg) => {
                onResult?.Invoke(false, errorMsg);
            });
    }

    // --- 2. 데이터 저장 및 로드 ---

    public IEnumerator SaveGameData(GameData data, Action onComplete = null)
    {
        if (string.IsNullOrEmpty(authToken)) { yield break; }
        string json = JsonUtility.ToJson(data);

        yield return SendRequest("/my-data", "PATCH", data,
            (successJson) => {
                Debug.Log("데이터 저장 완료");
                onComplete?.Invoke();
            },
            (errorMsg) => {
                Debug.LogError("저장 실패: " + errorMsg);
                onComplete?.Invoke(); // 실패해도 흐름은 계속되게 함
            });
    }

    public IEnumerator LoadGameData(Action<GameData> onDataLoaded)
    {
        if (string.IsNullOrEmpty(authToken)) { onDataLoaded?.Invoke(null); yield break; }

        yield return SendRequest("/my-data", "GET", null,
            (json) => {
                var data = JsonUtility.FromJson<GameData>(json);
                onDataLoaded?.Invoke(data);
            },
            (errorMsg) => {
                Debug.LogError("로드 실패: " + errorMsg);
                onDataLoaded?.Invoke(null);
            });
    }

    // --- 3. 통신 핵심 로직 (에러 파싱 포함) ---
    private IEnumerator SendRequest(string path, string method, object data, Action<string> onSuccess, Action<string> onFail = null)
    {
        string url = baseUrl + path;
        string json = data != null ? JsonUtility.ToJson(data) : null;

        using (UnityWebRequest req = new UnityWebRequest(url, method))
        {
            if (json != null)
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
                req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(authToken))
                req.SetRequestHeader("Authorization", "Bearer " + authToken);

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(req.downloadHandler.text);
            }
            else
            {
                string serverMsg = "네트워크 오류";
                try
                {
                    if (!string.IsNullOrEmpty(req.downloadHandler.text))
                    {
                        var errorRes = JsonUtility.FromJson<ErrorResponse>(req.downloadHandler.text);
                        if (errorRes != null && !string.IsNullOrEmpty(errorRes.message))
                        {
                            serverMsg = errorRes.message;
                        }
                    }
                }
                catch
                {
                    serverMsg = req.error;
                }

                Debug.LogError($"[{method}] {path} 실패: {serverMsg}");
                onFail?.Invoke(serverMsg);
            }
        }
    }


    [Serializable]
    class UserLoginData
    {
        public string username;
        public string password;
    }

    [Serializable]
    class UserRegisterData
    {
        public string username;
        public string password;
        public string nickname;
    }

    [Serializable]
    class LoginResponse
    {
        public string message;
        public string token;
    }

    [Serializable]
    public class ErrorResponse
    {
        public string message;
    }
}