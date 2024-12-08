using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSword", menuName = "Weapon/SwordData")]
public class SwordData : WeaponData
{
    // Start is called before the first frame update
    public override void Upgrade()
    {
        upgradeCount++;

        // 업그레이드 효과 적용
        switch (upgradeCount)
        {
            case 1:
                attackPower += 1f; // 첫 번째 업그레이드: 공격력 증가, 공격 범위 증가
                attackRange += 1f;
                Debug.Log($"{weaponName}의 공격력이 {attackPower}로 증가했습니다.");
                break;
            case 2:
                attackSpeed += 0.5f; // 두 번째 업그레이드: 공격속도 증가, 공격 범위 증가
                attackRange += 1f;
                Debug.Log($"{weaponName}의 공격속도가 {attackSpeed}로 증가했습니다.");
                break;
            case 3:
                attackRange += 1.5f; // 세 번째 업그레이드: 공격범위 증가, 공격력 증가
                attackPower += 1f;
                Debug.Log($"{weaponName}의 공격범위가 {attackRange}로 증가했습니다.");
                break;
            case 4:
                attackPower += 4f;  // 네 번째 업그레이드: 공격력 증가, 공격속도 증가, 공격 범위 증가
                attackRange += 2f;
                attackSpeed += 1.5f;
                Debug.Log($"{weaponName}의 공격속도가 {attackSpeed}로 증가했습니다.");
                break;
            default:
                Debug.Log($"{weaponName}의 업그레이드가 더 이상 적용되지 않습니다.");
                break;
        }
    }
    public override GameObject CreatePrefab(Vector3 position, Quaternion rotation)
    {
        // Fireball 프리팹을 생성
        GameObject attack = Instantiate(weaponPrefab, position, rotation);

        // 프리팹에 `WeaponData`를 연결
        SlashScript bulletScript = attack.GetComponent<SlashScript>();
        if (bulletScript != null)
        {
            bulletScript.weaponData = this; // 자신의 데이터를 전달
        }

        return attack;  // 생성된 프리팹 반환
    }
}
