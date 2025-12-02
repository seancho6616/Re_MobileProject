using UnityEngine;

public class BossEventManager : MonoBehaviour
{
    public BossController bossController;
    public ParticleSystem EventEffect;
    private int pressedButtonCount = 0; // 눌린 버튼 수

    // 버튼이 눌리면 이 함수가 실행됨 - StatueButton 호출
    public void ButtonPressed()
    {
        pressedButtonCount++;
        Debug.Log("버튼 눌림! 현재: " + pressedButtonCount + " / 3");

        if (EventEffect != null)
        {
            EventEffect.Play();
        }
    }
    
    public void TryActivateBoss()
    {
        if (pressedButtonCount == 3 && bossController.isActivated == false)
        {
            Debug.Log("보스 활성화");
            bossController.isActivated = true;
        }
        else if (pressedButtonCount < 3)
        {
            Debug.Log("아직 버튼이 모두 눌리지 않았음 (" + pressedButtonCount + " / 3)");
        }
    }
}