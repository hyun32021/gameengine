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

    public Sprite weaponImage; // ���� �̹���
    // ���� Ÿ�̸� �ʱ�ȭ
    public void ResetAttackTimer()
    {
        attackTimer = 0f;
    }

    // ���� ���׷��̵� �޼���
    public void Upgrade()
    {
        // ����: ���� �ӵ�, ����, ���ݷ� ����
        attackSpeed *= 1.1f;  // ���� �ӵ��� 10% ����
        attackRange *= 1.1f;  // ���� ������ 10% ����
        attackPower *= 1.1f;  // ���ݷ��� 10% ����

        Debug.Log($"{weaponName}�� ���׷��̵�Ǿ����ϴ�! ���ο� ���� �ӵ�: {attackSpeed}, ���� ����: {attackRange}, ���ݷ�: {attackPower}");
    }
}

