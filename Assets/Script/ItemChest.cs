using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ChestData
{
    public string itemName;
    public GameObject itemPrefab;

    [Range(0, 100)]
    public int dropChance;

    public int minAmount = 1;
    public int maxAmount = 1;
}

public class ItemChest : MonoBehaviour
{
    [Header("설정")]
    public Transform dropPoint;
    public float spreadForce = 3f;
    public Animator chestAnimator;

    [Header("드랍할 아이템 목록")]
    public List<ChestData> dataTable = new List<ChestData>();

    private bool isOpened = false;

    // 플레이어가 클릭 시 실행
    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     if (!isOpened)
    //     {
    //         OpenChest();
    //     }
    // }
    public void OpenChest()
    {
        if (isOpened) return;

        isOpened = true;
        Debug.Log("chest Open");

        if (chestAnimator != null)
        {
            chestAnimator.SetTrigger("Open");
        }

        // 아이템 드랍 로직
        foreach (ChestData drop in dataTable)
        {
            TryDropItem(drop);
        }

        // 상자 중복 열기 방지
        Collider col = GetComponent<Collider>();
        if(col != null) col.enabled = false;
    }

    // 개별 아이템 드랍 로직
    void TryDropItem (ChestData drop)
    {
        // 0~100 사이 수를 하나 뽑아 확률과 비교
        int randomValue = Random.Range(0, 101);

        // 당첨 및 확률 100일 경우
        if(randomValue <= drop.dropChance)
        {
            int countToSpawn = Random.Range(drop.minAmount, drop.maxAmount +1);

            for (int i=0; i<countToSpawn; i++)
            {
                SpawnItem(drop.itemPrefab);
            }
        }
    }

    // 실제 아이템 생성 및 물리 효과
    void SpawnItem(GameObject prefab)
    {
        if (prefab == null) return ;

        //아이템 생성 / 위치 랜덤화로 겹침 최소화
        Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
        randomOffset.y = 0;

        GameObject item = Instantiate(prefab, dropPoint.position + randomOffset, Quaternion.identity);

        //튀어나가는 효과
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 위쪽 + 랜덤 방향
            Vector3 forceDir = Vector3.up + Random.insideUnitSphere;
            rb.AddForce(forceDir.normalized * spreadForce, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * spreadForce, ForceMode.Impulse);
        }
    }
}
