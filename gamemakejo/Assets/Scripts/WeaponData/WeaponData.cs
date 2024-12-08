using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    public string weaponName;
    public bool weaponType; // true: 원거리, false: 근거리
    public GameObject weaponPrefab;
    public GameObject projectilePrefab;
    public float attackSpeed;
    public float attackRange;
    public float attackPower;
    public int upgradeCount = 0; // 업그레이드 횟수 카운트

    private float attackTimer;

    public Sprite weaponImage;

    // Equals 메서드 오버라이드: 무기 이름만으로 비교
    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is WeaponData))
            return false;

        WeaponData other = (WeaponData)obj;
        return this.weaponName == other.weaponName; // 무기 이름으로 비교
    }

    public override int GetHashCode()
    {
        return weaponName.GetHashCode(); // 무기 이름을 기준으로 해시 코드 생성
    }
    // 무기 업그레이드: 각 무기별로 구현
    public abstract void Upgrade();

    // 추상 메서드: 각 무기별로 프리팹을 생성하고 데이터를 전달하도록 처리
    public abstract GameObject CreatePrefab(Vector3 position, Quaternion rotation);
    // 공격 타이머 초기화
    public void ResetAttackTimer()
    {
        attackTimer = 0f;
    }
}
