using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float damage = 1f;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();

            if (stats != null)
            {
                stats.CurrentHealth -= damage;
                
                Debug.Log("남은 체력: " + stats.CurrentHealth);
            }

            // 4. 총알 삭제
            Destroy(gameObject);
        }
    }
}