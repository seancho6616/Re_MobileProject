using System;

[Serializable]
public class GameData
{
    public bool hasPlayed; 

    // 씬 및 위치 정보
    public string lastScene;
    public PositionData position; 

    // 아이템 및 재화
    public int coins;
    public int potionCount;

    // 플레이어 스탯 (체력, 스태미너)
    public int maxHeart;
    public int currentHeart;
    public int maxStamina;
    public int currentStamina;
    
    // 전투 및 이동 스탯
    public int speed;
    public int baseAttack;
    public int baseAttackSpeed;
    public int attackRange;

    // 장비 (무기 ID)
    public int equippedWeaponId; 
}

// 좌표(Vector3)를 JSON으로 저장하기 위한 보조 클래스
[Serializable]
public class PositionData
{
    public float x;
    public float y;
    public float z;

    public PositionData(float x, float y, float z) 
    { 
        this.x = x; 
        this.y = y; 
        this.z = z; 
    }
}