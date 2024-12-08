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

        // ���׷��̵� ȿ�� ����
        switch (upgradeCount)
        {
            case 1:
                attackPower += 5; // ù ��° ���׷��̵�: ���ݷ� ����
                Debug.Log($"{weaponName}�� ���ݷ��� {attackPower}�� �����߽��ϴ�.");
                break;
            case 2:
                attackSpeed *= 1.1f; // �� ��° ���׷��̵�: ���ݼӵ� ����
                Debug.Log($"{weaponName}�� ���ݼӵ��� {attackSpeed}�� �����߽��ϴ�.");
                break;
            case 3:
                attackRange += 2f; // �� ��° ���׷��̵�: ���ݹ��� ����
                Debug.Log($"{weaponName}�� ���ݹ����� {attackRange}�� �����߽��ϴ�.");
                break;
            default:
                Debug.Log($"{weaponName}�� ���׷��̵尡 �� �̻� ������� �ʽ��ϴ�.");
                break;
        }
    }
}
