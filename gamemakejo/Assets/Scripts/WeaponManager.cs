using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponData> equippedWeapons = new List<WeaponData>(); // ���� ������ ���� ����Ʈ
    public Transform weaponSlotParent; // ������� ��ġ�� �θ� ��ü
    public Transform playerPosition; // ������ ���� ���� (�÷��̾� ��ġ)
    public LayerMask enemyLayer; // ���� ���� ���̾�

    private List<float> attackTimers = new List<float>(); // ���⺰ ���� Ÿ�̸�

    public List<WeaponData> allWeapons; // ��� ���� ����Ʈ

    public int maxWeaponSlots = 4; // �ִ� ���� ������ ���� ���� ��

    // ������ ���� ����Ʈ ��ȯ
    public List<WeaponData> GetEquippedWeapons()
    {
        return equippedWeapons;
    }

    // ���� ������ �� �ִ� ���� ����Ʈ ��ȯ
    public List<WeaponData> GetAvailableWeapons()
    {
        return allWeapons.Where(w => !equippedWeapons.Contains(w)).ToList();
    }

    void Start()
    {
        // ������ ����鿡 ���� �ʱ�ȭ �۾��� ���ۿ��� �� ���� ����
        foreach (var weapon in equippedWeapons)
        {
            // ������ ������ ���� Ÿ�̸� �ʱ�ȭ
            weapon.ResetAttackTimer();
        }

        // ���� Ÿ�̸� ����Ʈ ũ�� �ʱ�ȭ
        attackTimers = new List<float>(new float[equippedWeapons.Count]);
    }

    void Update()
    {
        // ���� Ÿ�̸Ӱ� equippedWeapons ����Ʈ�� ũ�Ⱑ ��ġ�ϵ��� ����
        if (attackTimers.Count != equippedWeapons.Count)
        {
            attackTimers = new List<float>(new float[equippedWeapons.Count]);
        }

        // �� ������ ���� ����
        for (int i = 0; i < equippedWeapons.Count; i++)
        {
            attackTimers[i] += Time.deltaTime;

            if (attackTimers[i] >= 1 / equippedWeapons[i].attackSpeed)
            {
                Attack(equippedWeapons[i]);
                attackTimers[i] = 0; // Ÿ�̸� �ʱ�ȭ
            }
        }
    }

    // ���� ���� ���
    public void EquipWeapon(WeaponData weapon)
    {
        // �̹� ������ ���� ����Ʈ���� �ش� ���Ⱑ �ִ��� Ȯ�� (���� �̸��� ��)
        foreach (var equippedWeapon in equippedWeapons)
        {
            if (equippedWeapon.weaponName == weapon.weaponName)
            {
                Debug.LogWarning($"{weapon.weaponName}�� �̹� ������ �����Դϴ�!");
                return;  // �̹� ������ ������ �߰����� ����
            }
        }

        // �ִ� ���� ���� Ȯ��
        if (equippedWeapons.Count >= maxWeaponSlots)
        {
            Debug.LogWarning("���� ������ ���� á���ϴ�!");
            return;
        }

        // ���� ������ �߰�
        equippedWeapons.Add(weapon);

        // ���� Ÿ�̸� ����Ʈ ũ�� ����
        attackTimers.Add(0f);  // ���ο� Ÿ�̸� �ʱ�ȭ

        // ���ο� ������ Ÿ�̸� �ʱ�ȭ
        weapon.ResetAttackTimer();

        // ���� ������ ����
        GameObject newWeapon = Instantiate(weapon.weaponPrefab, weaponSlotParent);
        Debug.Log($"{weapon.weaponName} ���� �Ϸ�! ���� ������ ���� ��: {equippedWeapons.Count}");
    }

    private void Attack(WeaponData weapon)
    {
        if (weapon.projectilePrefab != null)
        {
            if (weapon.weaponType)
            {
                // ���Ÿ� ����: ����ü �߻�
                GameObject projectile = Instantiate(weapon.projectilePrefab, playerPosition.position, playerPosition.rotation);
            }
            else if (!weapon.weaponType)
            {
                // �ٰŸ� ����: ������ ����
                GameObject projectile = Instantiate(weapon.projectilePrefab, playerPosition.position, playerPosition.rotation);
                projectile.transform.SetParent(playerPosition);
            }
            else { Debug.LogError("�߸��� weaponType ���� ���޵Ǿ����ϴ�. weaponType�� true �Ǵ� false���� �մϴ�. "); }
        }
       
    }
  
}
