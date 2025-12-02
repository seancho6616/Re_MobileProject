using UnityEngine;

public class BossZone : MonoBehaviour
{
    public BossEventManager eventManager;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (eventManager != null) eventManager.TryActivateBoss();
        }
    }
}
