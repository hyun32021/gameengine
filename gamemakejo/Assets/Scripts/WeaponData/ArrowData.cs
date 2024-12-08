using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArrow", menuName = "Weapon/ArrowData")]
public class ArrowData : WeaponData
{
    public bool isMultiShot = false; // 다중 발사 여부
    public int penetration = 0;
    // Start is called before the first frame update
    public override void Upgrade()
    {
        upgradeCount++;

        // 업그레이드 효과 적용
        switch (upgradeCount)
        {
            case 1:
                attackSpeed += 0.5f; // 첫 번째 업그레이드: 공격속도 증가
                Debug.Log($"{weaponName}의 공격속도가 {attackSpeed}로 증가했습니다.");
                break;
            case 2:
                attackSpeed += 0.5f; // 두 번째 업그레이드: 공격속도 증가, 관통력 증가
                penetration++;
                Debug.Log($"{weaponName}의 공격속도가 {attackSpeed}로 증가했습니다.");
                break;
            case 3:
                attackPower += 1f; // 세 번째 업그레이드: 공격력 증가, 공격속도 증가
                attackSpeed += 0.5f;
                Debug.Log($"{weaponName}의 공격력이 {attackPower}로 증가했습니다.");
                break;
            case 4:
                isMultiShot = true; // 네 번째 업그레이드: 투사체 증가, 관통력 증가, 공격속도 증가
                penetration++;
                attackSpeed += 1f;
                Debug.Log($"{weaponName}이(가) 두 발 동시 발사 기능을 활성화했습니다.");
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
        BulletCtrl bulletScript = attack.GetComponent<BulletCtrl>();
        if (bulletScript != null)
        {
            bulletScript.weaponData = this; // 자신의 데이터를 전달
        }

        return attack;  // 생성된 프리팹 반환
    }
}
