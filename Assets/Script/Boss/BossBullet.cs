using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float damage = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // [수정됨] PlayerStats 대신 PlayerControl 스크립트를 가져옵니다.
            // 그래야 우리가 만든 안전한 Damaged 함수를 호출할 수 있습니다.
            PlayerControl playerControl = other.GetComponent<PlayerControl>();

            if (playerControl != null)
            {
                // [핵심 수정] 변수에 직접 접근하지 않고, 안전한 피격 함수를 호출합니다.
                // 이 함수 안에 체력 고정 및 Game Over 로직이 들어 있습니다.
                playerControl.Damaged(damage);
            }

            // 총알 삭제
            Destroy(gameObject);
        }
    }
}