using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLightningball", menuName = "Weapon/LightningballData")]
public class LightningballData : WeaponData
{
    public int chainCount = 3;
    public override void Upgrade()
    {
        upgradeCount++;

        // 업그레이드 효과 적용
        switch (upgradeCount)
        {
            case 1:
                attackPower += 1; // 첫 번째 업그레이드: 공격력 증가
                Debug.Log($"{weaponName}의 공격력이 {attackPower}로 증가했습니다.");
                break;
            case 2:
                chainCount++; ; // 두 번째 업그레이드: 연쇄 횟수 증가, 공격력 증가
                attackPower += 1;
                Debug.Log($"{weaponName}의 공격속도가 {attackSpeed}로 증가했습니다.");
                break;
            case 3:
                chainCount += 2;   // 세 번째 업그레이드: 연쇄 횟수 증가, 공격력 증가
                attackPower += 1;  
                Debug.Log($"{weaponName}의 공격범위가 {attackRange}로 증가했습니다.");
                break;
            case 4:
                chainCount += 5;   // 네 번째 업그레이드: 연쇄 횟수 증가, 공격력 증가, 연쇄 범위 증가, 공격 속도 증가
                attackPower += 3;
                attackRange = 30f;
                attackSpeed += 1f;
                Debug.Log($"{weaponName}의 공격범위가 {attackRange}로 증가했습니다.");
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
        LightningBall bulletScript = attack.GetComponent<LightningBall>();
        if (bulletScript != null)
        {
            bulletScript.weaponData = this; // 자신의 데이터를 전달
        }

        return attack;  // 생성된 프리팹 반환
    }
}
