using UnityEngine;

public class BossController : MonoBehaviour
{
    public bool isActivated = false; // ★ 처음엔 꺼져 있어야 함 (false)

    public Transform player;
    public float rotationSpeed = 20f;
    public float attackInterval = 1.5f;

    public BossAttack basicAttack; //보스 공격 스크립트 연결

    private float timer = 0f;

    void Update()
    {
        // 스위치 꺼져 있으면 멈춤
        if (isActivated == false) return;
        
        if (player == null) return;

        // 회전 로직
        Vector3 dir = player.position - transform.position;
        dir.y = 0; 
        
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }

        // 공격 타이머
        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            timer = 0f;
            
            if (basicAttack != null) 
            {
                basicAttack.Fire(player.position);
            }
        }
    }
}