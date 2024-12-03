using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        return allWeapons.Where(w => !equippedWeapons.Contains(w)).ToList();
    }

    void Start()
    {
        // 장착된 무기들에 대한 초기화 작업을 시작에서 한 번만 진행
        foreach (var weapon in equippedWeapons)
        {
            // 장착된 무기의 공격 타이머 초기화
            weapon.ResetAttackTimer();
        }

        // 공격 타이머 리스트 크기 초기화
        attackTimers = new List<float>(new float[equippedWeapons.Count]);
    }

    void Update()
    {
        // 공격 타이머가 equippedWeapons 리스트와 크기가 일치하도록 보장
        if (attackTimers.Count != equippedWeapons.Count)
        {
            attackTimers = new List<float>(new float[equippedWeapons.Count]);
        }

        // 각 무기의 공격 실행
        for (int i = 0; i < equippedWeapons.Count; i++)
        {
            attackTimers[i] += Time.deltaTime;

            if (attackTimers[i] >= 1 / equippedWeapons[i].attackSpeed)
            {
                Attack(equippedWeapons[i]);
                attackTimers[i] = 0; // 타이머 초기화
            }
        }
    }

    // 무기 장착 기능
    public void EquipWeapon(WeaponData weapon)
    {
        // 이미 장착된 무기 리스트에서 해당 무기가 있는지 확인 (무기 이름만 비교)
        foreach (var equippedWeapon in equippedWeapons)
        {
            if (equippedWeapon.weaponName == weapon.weaponName)
            {
                Debug.LogWarning($"{weapon.weaponName}는 이미 장착된 무기입니다!");
                return;  // 이미 장착된 무기라면 추가하지 않음
            }
        }

        // 최대 슬롯 제한 확인
        if (equippedWeapons.Count >= maxWeaponSlots)
        {
            Debug.LogWarning("무기 슬롯이 가득 찼습니다!");
            return;
        }

        // 무기 데이터 추가
        equippedWeapons.Add(weapon);

        // 공격 타이머 리스트 크기 갱신
        attackTimers.Add(0f);  // 새로운 타이머 초기화

        // 새로운 무기의 타이머 초기화
        weapon.ResetAttackTimer();

        // 무기 프리팹 생성
        GameObject newWeapon = Instantiate(weapon.weaponPrefab, weaponSlotParent);
        Debug.Log($"{weapon.weaponName} 장착 완료! 현재 장착된 무기 수: {equippedWeapons.Count}");
    }

    private void Attack(WeaponData weapon)
    {
        if (weapon.projectilePrefab != null)
        {
            // 원거리 공격: 투사체 발사
            GameObject projectile = Instantiate(weapon.projectilePrefab, playerPosition.position, playerPosition.rotation);
            /*
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
               // rb.velocity = transform.forward * 10f; // 투사체 발사 속도
            }
            */
        }
        else
        {
            // 근거리 공격: 적 탐지 후 데미지
            Collider[] hitEnemies = Physics.OverlapSphere(playerPosition.position, weapon.attackRange, enemyLayer);
            foreach (var enemy in hitEnemies)
            {
                // 적에게 데미지 입히기
                Debug.Log($"{enemy.name}에게 {weapon.attackPower}의 데미지를 입혔습니다!");
                // 적에 데미지를 주는 코드 추가 필요 (e.g., enemy.TakeDamage(weapon.attackPower))
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 근거리 무기 범위를 시각화
        Gizmos.color = Color.red;
        foreach (var weapon in equippedWeapons)
        {
            Gizmos.DrawWireSphere(playerPosition.position, weapon.attackRange);
        }
    }
}
