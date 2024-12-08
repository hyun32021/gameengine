using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFireball", menuName = "Weapon/FireballData")]
public class FireballData : WeaponData
{
    public bool isRapidFire = false; // 연속 발사 여부
    public override void Upgrade()
    {
        upgradeCount++;

        // 업그레이드 효과 적용
        switch (upgradeCount)
        {
            case 1:
                attackPower += 1f; // 첫 번째 업그레이드: 공격력 증가
                Debug.Log($"{weaponName}의 공격력이 {attackPower}로 증가했습니다.");
                break;
            case 2:
                attackPower += 1f; // 두 번째 업그레이드: 공격력 증가, 공격범위 증가
                attackRange += 1f; 
                Debug.Log($"{weaponName}의 공격속도가 {attackSpeed}로 증가했습니다.");
                break;
            case 3:
                attackPower += 1f;  // 세 번째 업그레이드: 공격범위 증가, 공격력 증가
                attackRange += 1f; 
                Debug.Log($"{weaponName}의 공격범위가 {attackRange}로 증가했습니다.");
                break;
            case 4:
                attackPower += 4f;  // 네 번째 업그레이드: 공격력 증가, 공격 범위 증가
                attackRange += 3f;
                Debug.Log($"{weaponName}의 공격속도가 {attackSpeed}로 증가했습니다.");
                break;
            default:
                Debug.Log($"{weaponName}의 업그레이드가 더 이상 적용되지 않습니다.");
                break;
        }
    }
    // Fireball 프리팹을 생성하고 데이터를 전달하는 메서드
    public override GameObject CreatePrefab(Vector3 position, Quaternion rotation)
    {
        // Fireball 프리팹을 생성
        GameObject fireball = Instantiate(weaponPrefab, position, rotation);

        // 프리팹에 `WeaponData`를 연결
        Fireball fireballScript = fireball.GetComponent<Fireball>();
        if (fireballScript != null)
        {
            fireballScript.weaponData = this; // 자신의 데이터를 전달
        }

        return fireball;  // 생성된 프리팹 반환
    }
}
