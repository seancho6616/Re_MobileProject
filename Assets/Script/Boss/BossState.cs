using UnityEngine;

public class BossState : MonoBehaviour
{
    public int maxHealth = 32;
    private int currentHealth;

    public float damageCooldown = 1.0f;
    private float lastDamageTime = 0;

    public Animator playerAnimator; 

    public string realAttackName = "Armature_Attack1";

    void Start()
    {
        currentHealth = maxHealth;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            CheckAttack();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            CheckAttack();
        }
    }

    void CheckAttack()
    {
        if (playerAnimator == null) return;

        AnimatorClipInfo[] clipInfo = playerAnimator.GetCurrentAnimatorClipInfo(0);

        if (clipInfo.Length > 0)
        {
            if (clipInfo[0].clip.name == realAttackName)
            {
                if (Time.time >= lastDamageTime + damageCooldown)
                {
                    TakeDamage(6);
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("공격 성공. 남은 체력: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("보스 사망");
        Destroy(gameObject);
    }
}