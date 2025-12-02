using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint;     // 발사 위치 (BellyPos)
    public float bulletSpeed = 20f;   // 총알 속도

    public void Fire(Vector3 targetPos)
    {
        if (bulletPrefab == null || firePoint == null) return;

        // 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // 플레이어 방향 계산 (수평 발사)
        Vector3 direction = targetPos - firePoint.position;
        direction.y = 0; // 바닥으로 꺼짐 방지
        direction = direction.normalized;

        // 발사 힘 적용
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        Destroy(bullet, 5f);
    }
}