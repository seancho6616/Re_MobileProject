using UnityEngine;

public class Sound : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        audioSource.Play();
    }
}
