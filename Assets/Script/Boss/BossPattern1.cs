using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Security.Cryptography;
using UnityEditor.Callbacks;

public class BossPattern1 : MonoBehaviour
{
    public GameObject bulltetPrefab;
    public Transform firePoint;

    // 난이도 설정
    public float bulletSpeed = 10f;
    public int armCount = 4;
    public float rotationSpeed = 20;
    public float duration = 6f;

    // 총알 사이 구멍 범위
    public float fireRate = 0.7f;
    public void CastPattern()
    {
        StartCoroutine (CrossBarRoutine());
    }

    IEnumerator CrossBarRoutine() // 회전 발사 패턴
    {
        float currentAngle = 0f;
        float timer = 0f;

        // duration: 패턴 지속 시간
        while (timer < duration)
        {
            for (int i = 0; i < armCount; i++)
            {
                // 각도 계산
                float angle = currentAngle + (i * (360f / armCount));
                // 각도 변환
                Quaternion rot = Quaternion.Euler(0, angle, 0);
                // 날아갈 방향 계산
                Vector3 dir = rot * transform.forward;
                // 총알 생성
                GameObject bullet = Instantiate(bulltetPrefab, firePoint.position + new Vector3(1f, 1f, 1f), Quaternion.identity);
                // 총알 발사
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // 방향을 속도만큼 날려보냄
                    rb.linearVelocity = dir * bulletSpeed;
                }
                Destroy(bullet, 10f);
            }
            // 각도 업데이트 - 속도 * 시간 만큼 각도 증가
            currentAngle += rotationSpeed * fireRate;

            // 시간 누적
            timer += fireRate;

            // 총알 사이로 지나갈 공간을 만들기 위한 빈틈 생성
            yield return new WaitForSeconds(fireRate);
        }
    }
}
