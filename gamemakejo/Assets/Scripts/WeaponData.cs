using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public GameObject projectilePrefab;
    public float attackSpeed;
    public float attackRange;
    public float attackPower;

    private float attackTimer;

    public Sprite weaponImage; // 무기 이미지
    // 공격 타이머 초기화
    public void ResetAttackTimer()
    {
        attackTimer = 0f;
    }

    // 무기 업그레이드 메서드
    public void Upgrade()
    {
        // 예시: 공격 속도, 범위, 공격력 증가
        attackSpeed *= 1.1f;  // 공격 속도를 10% 증가
        attackRange *= 1.1f;  // 공격 범위를 10% 증가
        attackPower *= 1.1f;  // 공격력을 10% 증가

        Debug.Log($"{weaponName}가 업그레이드되었습니다! 새로운 공격 속도: {attackSpeed}, 공격 범위: {attackRange}, 공격력: {attackPower}");
    }
}

