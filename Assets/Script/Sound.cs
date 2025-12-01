using UnityEngine;
using UnityEngine.SceneManagement;

public class Sound : MonoBehaviour
{
    private static Sound instance; // 싱글톤 인스턴스
    private AudioSource audioSource;

    [Header("BGM Clips")]
    public AudioClip menuBGM; // 로그인 & 시작 화면용 음악
    public AudioClip mainBGM; // 메인 게임용 음악

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. 재생해야 할 음악 결정
        AudioClip targetClip = null;

        if (scene.name == "LoginScene" || scene.name == "StartScene")
        {
            targetClip = menuBGM; // 메뉴 음악
        }
        else if (scene.name == "MainScene")
        {
            targetClip = mainBGM; // 게임 음악
        }

        // 2. 음악 교체 로직 
        if (targetClip != null)
        {
            // Login -> Start
            if (audioSource.clip != targetClip) 
            {
                audioSource.clip = targetClip;
                audioSource.Play();
            }
        }
    }

    public void PlaySound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}