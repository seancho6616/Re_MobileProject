using UnityEngine;
using System.Collections;

public class StatueButton : MonoBehaviour
{
    // 버튼 개수 관리 함수
    public BossEventManager eventManager;

    // 버튼이 한번 눌리면 다시 못 누름
    private bool isPressed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPressed)
        {
            isPressed = true;
            
            // 버튼이 회색으로 변하고 아래로 들어가는 기능
            GetComponent<Renderer>().material.color = Color.gray; 
            transform.position += Vector3.down * 0.2f; 

            // eventManager 연동 (보스에게 알림)
            if(eventManager != null) eventManager.ButtonPressed();
        }
    }
}