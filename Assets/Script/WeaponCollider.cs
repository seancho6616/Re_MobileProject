using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    PlayerControl playerControl;
    MonsterAI monsterAI;
    void Start()
    {
        playerControl = FindAnyObjectByType<PlayerControl>();
    }
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Monster"))
        {
            playerControl.isMonster = true;
        }
        if(other.TryGetComponent(out MonsterAI monster))
        {
            playerControl.monsterAI = monster;
            Debug.Log(other.gameObject.name);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            playerControl.isMonster = false;
        }
        playerControl.monsterAI = null;
    }
    
}
