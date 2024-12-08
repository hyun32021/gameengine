using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    public string weaponName;
    public bool weaponType; // true: ���Ÿ�, false: �ٰŸ�
    public GameObject weaponPrefab;
    public GameObject projectilePrefab;
    public float attackSpeed;
    public float attackRange;
    public int attackPower;
    public int upgradeCount = 0; // ���׷��̵� Ƚ�� ī��Ʈ

    private float attackTimer;

    public Sprite weaponImage;

    // ���� ���׷��̵�: �� ���⺰�� ����
    public abstract void Upgrade();

    // ���� Ÿ�̸� �ʱ�ȭ
    public void ResetAttackTimer()
    {
        attackTimer = 0f;
    }
}
