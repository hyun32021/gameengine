using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponData> equippedWeapons = new List<WeaponData>(); // 현재 장착된 무기 리스트
    public Transform weaponSlotParent; // 무기들이 배치될 부모 객체
    public Transform playerPosition; // 공격의 시작 지점 (플레이어 위치)
    public LayerMask enemyLayer; // 적이 속한 레이어

    private List<float> attackTimers = new List<float>(); // 무기별 공격 타이머

    public List<WeaponData> allWeapons; // 모든 무기 리스트
    public int maxWeaponSlots = 4; // 최대 장착 가능한 무기 슬롯 수

    // 장착된 무기 리스트 반환
    public List<WeaponData> GetEquippedWeapons()
    {
        return equippedWeapons;
    }

    // 새로 장착할 수 있는 무기 리스트 반환
    public List<WeaponData> GetAvailableWeapons()
    {
        return allWeapons.FindAll(w => !equippedWeapons.Contains(w));
    }

    void Start()
    {
        foreach (var weapon in equippedWeapons)
        {
            weapon.ResetAttackTimer();
        }
        attackTimers = new List<float>(new float[equippedWeapons.Count]);
    }

    void Update()
    {
        if (attackTimers.Count != equippedWeapons.Count)
        {
            attackTimers = new List<float>(new float[equippedWeapons.Count]);
        }

        for (int i = 0; i < equippedWeapons.Count; i++)
        {
            attackTimers[i] += Time.deltaTime;

            if (attackTimers[i] >= 1 / equippedWeapons[i].attackSpeed)
            {
                Attack(equippedWeapons[i]);
                attackTimers[i] = 0;
            }
        }
    }

    public void EquipWeapon(WeaponData weapon)
    {
        // 이미 장착된 무기 리스트에서 해당 무기가 있는지 확인
        foreach (var equippedWeapon in equippedWeapons)
        {
            if (equippedWeapon.weaponName == weapon.weaponName)
            {
                Debug.LogWarning($"{weapon.weaponName}는 이미 장착된 무기입니다!");
                return;  // 이미 장착된 무기라면 추가하지 않음
            }
        }
        if (equippedWeapons.Count >= maxWeaponSlots)
        {
            Debug.LogWarning("무기 슬롯이 가득 찼습니다!");
            return;
        }

        WeaponData runtimeWeapon = Instantiate(weapon); // 런타임 복제
        equippedWeapons.Add(runtimeWeapon);
        attackTimers.Add(0f);

        // 새로운 무기의 타이머 초기화
        runtimeWeapon.ResetAttackTimer();

        Debug.Log($"{runtimeWeapon.weaponName} 장착 완료!");
    }

    private void Attack(WeaponData weapon)
    {
        if (weapon.projectilePrefab != null)
        {
            if (weapon.weaponType)
            {
                // weaponName이 "Arrow"인 경우에만 추가 조건 처리
                switch (weapon.weaponName)
                {
                    case "Arrow":
                        // ArrowData 타입으로 형변환 후, 두 발 발사 여부 확인
                        if (weapon is ArrowData arrowData && arrowData.isMultiShot)
                        {
                            // 3발 발사 (중앙 + 좌우)
                            weapon.CreatePrefab(playerPosition.position, playerPosition.rotation); // 중앙 발사
                            weapon.CreatePrefab(playerPosition.position, playerPosition.rotation * Quaternion.Euler(0, -5, 0)); // 왼쪽 발사
                            weapon.CreatePrefab(playerPosition.position, playerPosition.rotation * Quaternion.Euler(0, 5, 0)); // 오른쪽 발사
                            return; // 다중 발사 처리 후 종료
                        }
                        break;
                        // 추후 다른 무기 타입을 추가하려면, 여기에 case 추가
                        // case "AnotherWeaponName":
                        //     // 해당 무기 조건 추가
                        //     break;
                }

                weapon.CreatePrefab(playerPosition.position, playerPosition.rotation);
            }
            else
            {
                // weaponType이 거짓일 경우 처리
                GameObject projectile = weapon.CreatePrefab(playerPosition.position, playerPosition.rotation);
                projectile.transform.SetParent(playerPosition);
            }
        }
    }
}

