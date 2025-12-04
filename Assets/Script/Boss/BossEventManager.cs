using UnityEngine;
using UnityEngine.UI;

public class BossEventManager : MonoBehaviour
{
    public BossController bossController;
    public ParticleSystem EventEffect;
    public Slider healthSlider;
    private int pressedButtonCount = 0; // 눌린 버튼 수

    // 버튼이 눌리면 이 함수가 실행됨 - StatueButton 호출
    public void ButtonPressed()
    {
        pressedButtonCount++;
        Debug.Log("버튼 눌림! 현재: " + pressedButtonCount + " / 3");

        if (EventEffect != null)
        {
            EventEffect.gameObject.SetActive(true);
            EventEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            EventEffect.Play();
        }
    }

    public void PlayerEnteredZone()
    {
        if (pressedButtonCount == 3)
        {
            healthSlider.gameObject.SetActive(true);
            Debug.Log("보스 구역 진입. 공격 시작");
            if (bossController != null) bossController.isActivated = true;
        }
    }

    public void PlayerExitedZone()
    {
        if (bossController != null && bossController.isActivated)
        {
            Debug.Log("보스 구역 이탈. 공격 중지...");
            bossController.isActivated = false;
            healthSlider.gameObject.SetActive(false);
        }
    }
}