using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    public string weaponName;
    public bool weaponType; // true: 원거리, false: 근거리
    public GameObject weaponPrefab;
    public GameObject projectilePrefab;
    public float attackSpeed;
    public float attackRange;
    public int attackPower;
    public int upgradeCount = 0; // 업그레이드 횟수 카운트

    private float attackTimer;

    public Sprite weaponImage;

    // 무기 업그레이드: 각 무기별로 구현
    public abstract void Upgrade();

    // 공격 타이머 초기화
    public void ResetAttackTimer()
    {
        attackTimer = 0f;
    }
}
