using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemState itemState;
    [SerializeField] float rotateSpeed;
    public AudioSource audioSource;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);        
    }
    public void PlaySound()
    {
        audioSource.Play();
    }
}
