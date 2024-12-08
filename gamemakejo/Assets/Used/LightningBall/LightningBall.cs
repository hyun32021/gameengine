using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : MonoBehaviour
{
    public float searchRadius = 10f;  // ���� ã�� ����
    private Transform closestEnemy;    // ���� ����� ���� Transform
    private List<Transform> previousEnemies = new List<Transform>();  // ������ �����ߴ� ������ ���
    public int maxChainCount = 3;  // �ִ� ���� Ƚ��
    public float moveSpeed = 30f;  // �̵� �ӵ�
    private Rigidbody rb;  // Rigidbody ������Ʈ
    [SerializeField]private int currentChainCount = 0;  // ���� ���� Ƚ��

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody ������Ʈ�� �����ɴϴ�.

        // ������ ���� �̺�Ʈ�� ����
        MonsterCtrl.OnMonsterDeath += CheckMonsters;
    }

    void OnDestroy()
    {
        // ���� ����
        MonsterCtrl.OnMonsterDeath -= CheckMonsters;
    }

    void Update()
    {
        // Ÿ���� �������� �ʾҴٸ�, ���� ����� ���� ã�´�
        if (closestEnemy == null || !closestEnemy.gameObject.activeInHierarchy)
        {
            // Ÿ���� ������ų� �׾�����, ���ο� Ÿ���� ã�´�
            FindClosestEnemy();
        }

        // Ÿ���� �����Ǿ� �ִٸ�, �� Ÿ���� �����Ѵ�
        if (closestEnemy != null)
        {
            MoveTowardsEnemy();
        }
    }

    void FindClosestEnemy()
    {
        // "Monster" �±׸� ���� ���� ã��
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        float minDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (GameObject monster in monsters)
        {
            // �̹� Ÿ���� ���ʹ� ����
            if (previousEnemies.Contains(monster.transform))
                continue;

            float distance = Vector3.Distance(transform.position, monster.transform.position);

            // searchRadius ���� ���͸� ����
            if (distance <= searchRadius && distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = monster.transform;
            }
        }

        // ���� ����� ���� ã����, �װ��� �����Ѵ�
        if (nearestEnemy != null)
        {
            closestEnemy = nearestEnemy;
            Debug.Log("���� ����� �� ����: " + closestEnemy.name);
        }
        else
        {
            // ��ȿ�� Ÿ���� ���� ���� Ƚ���� ���� �ִٸ� �������� �ı�
            if (currentChainCount < maxChainCount)
            {
                Destroy(gameObject);
                Debug.Log("��ȿ�� Ÿ���� �����Ƿ� LightningBall �������� �ı��մϴ�.");
            }
            else
            {
                Destroy(gameObject);
                Debug.Log("��ȿ�� Ÿ���� �����ϴ�. �׷��� ���Ⱑ ����Ǿ����ϴ�.");
            }
        }
    }



    void MoveTowardsEnemy()
    {
        // ���� closestEnemy�� null�̸�, �� �̻� ������ ���� �����Ƿ� �Լ��� ����
        if (closestEnemy == null)
            return;

        // ���µ� ���� ���� �̵�
        Vector3 direction = (closestEnemy.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;  // Rigidbody�� �̿��� ���������� �̵�
    }

    void OnTriggerEnter(Collider other)
    {
        // closestEnemy�� null�� �ƴϰ� "Monster" �±׸� ���� ���� �浹 ��, ���� ���µ� ������ ��
        if (closestEnemy != null && other.CompareTag("Monster") && other.transform == closestEnemy)
        {
            // �浹�� ���� ���� ���� ����
            Debug.Log(closestEnemy.name + "���� ��ҽ��ϴ�. ������ �����մϴ�.");

            // ���ظ� ����
            MonsterCtrl monsterCtrl = closestEnemy.GetComponent<MonsterCtrl>();
            if (monsterCtrl != null)
            {
                monsterCtrl.HP--;
                Debug.Log(closestEnemy.name + "���� 10�� ���ظ� �������ϴ�.");
            }

            // ù ��° Ÿ���� ���
            previousEnemies.Add(closestEnemy);  // �̹� Ÿ���� ���
            closestEnemy = null;  // �� �̻� ������ ���� �����Ƿ� null�� ����

            // ���� Ƚ�� üũ
            currentChainCount++;

            // �ִ� ���� Ƚ���� �����ߴٸ�, �� �̻� Ÿ���� ã�� �ʴ´�
            if (currentChainCount >= maxChainCount)
            {
                Debug.Log("�ִ� ���� Ƚ���� �����߽��ϴ�.");
                Destroy(gameObject);
            }

            // ���ο� Ÿ���� ã�´�
            FindClosestEnemy();
        }
    }


    // ���Ͱ� ���� ������ ���� ���͸� Ȯ���ϰ�, ��� ���Ͱ� �׾����� �ı�
    void CheckMonsters()
    {
        // "Monster" �±׸� ���� ������Ʈ�� ã�´�
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        // ���Ͱ� ������ �������� �ı�
        if (monsters.Length == 0 || currentChainCount >= maxChainCount)
        {
            Destroy(gameObject);  // ���� ��ü(������)�� �ı�
            Debug.Log("���Ͱ� ���ų� �ִ� ���� Ƚ���� ���������Ƿ� LightningBall �������� �ı��մϴ�.");
        }
    }
}