using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLightningball", menuName = "Weapon/LightningballData")]
public class LightningballData : WeaponData
{
    // Start is called before the first frame update
    public override void Upgrade()
    {
        upgradeCount++;

        // 업그레이드 효과 적용
        switch (upgradeCount)
        {
            case 1:
                attackPower += 5; // 첫 번째 업그레이드: 공격력 증가
                Debug.Log($"{weaponName}의 공격력이 {attackPower}로 증가했습니다.");
                break;
            case 2:
                attackSpeed *= 1.1f; // 두 번째 업그레이드: 공격속도 증가
                Debug.Log($"{weaponName}의 공격속도가 {attackSpeed}로 증가했습니다.");
                break;
            case 3:
                attackRange += 2f; // 세 번째 업그레이드: 공격범위 증가
                Debug.Log($"{weaponName}의 공격범위가 {attackRange}로 증가했습니다.");
                break;
            default:
                Debug.Log($"{weaponName}의 업그레이드가 더 이상 적용되지 않습니다.");
                break;
        }
    }
}
