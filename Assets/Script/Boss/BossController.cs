using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public bool isActivated = false;
    private bool isAttacking = false;

    public Transform player;
    public float rotationSpeed = 20f;
    public float attackInterval = 1.5f;

    public float patternDuration = 6f;

    public BossAttack basicAttack; //보스 공격 스크립트 연결
    public BossPattern1 pattern1; // 패턴1 스크립트 연결

    void LateUpdate()
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

        // 공격 결정 - 코루틴 사용, 공격중이 아닐 때 새로운 공격 시작
        if (!isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.5f); // 공격 전 잠시 대기

        int dice = Random.Range(0, 100);
        if (dice < 60)
        {
            Debug.Log("보스 기본 공격");
            if (basicAttack != null) basicAttack.Fire(player.position);
            yield return new WaitForSeconds(attackInterval);
        }
        else
        {
            Debug.Log("보스 패턴1 공격");
            if (pattern1 != null) pattern1.CastPattern();
            yield return new WaitForSeconds(patternDuration);
            yield return new WaitForSeconds(1f); 
        }
        isAttacking = false;
    }
}