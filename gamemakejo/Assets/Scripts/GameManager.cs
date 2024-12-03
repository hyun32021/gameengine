using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    public GameObject monsterPrefab;
    public GameObject monsterPrefab2;
    public GameObject bossPrefab; // ���� ���� ������

    public int numberOfMonsters = 5; // �� ���� ������ ���� ��
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 20f;
    public float spawnInterval = 5f; // ���� ���� �ֱ�
    public float spawnInterval_Boss = 300f; // ���� ���� ���� �ֱ�

    private float spawnTimer = 0f; // ��� �ð� ����
    private float spawnTimer_Boss = 0f;//���� ���� �����ð�
    private bool bossSpawned = false; // ���� ���� ���� Ȯ��

    public string nextSceneName;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else Instance = this;
    }

    void Update()
    {
        // ��� �ð��� �����ϰ� spawnInterval�� �����ϸ� ���� ��ȯ
        spawnTimer += Time.deltaTime;
        spawnTimer_Boss += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnMonsters();
            spawnTimer = 0f; // Ÿ�̸� �ʱ�ȭ
        }
        if (!bossSpawned)
        {
            if (spawnTimer_Boss >= spawnInterval_Boss)
            {
                SpawnBoss();
                bossSpawned = true; // ������ �� ���� �����ǵ��� ����
            }
        }
    }

    void SpawnMonsters()
    {
        for (int i = 0; i < numberOfMonsters; i++)
        {
            Vector3 spawnPosition = GenerateRandomGroundSpawnPoint();
            int monsterType = Random.Range(0, 2);
            if (monsterType == 0)
                Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            else
                Instantiate(monsterPrefab2, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnBoss()
    {
        if (!bossSpawned)
        {
            Vector3 spawnPosition = GenerateRandomGroundSpawnPoint();
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            bossSpawned = true; // ������ �� ���� �����ǵ��� ����
            Debug.Log("Boss Monster Spawned!");
        }
    }

    // Ground �±׸� ���� ������Ʈ ���� ���� ���� ����Ʈ ����
    Vector3 GenerateRandomGroundSpawnPoint()
    {
        // "Ground" �±׸� ���� ������Ʈ�� ã��
        GameObject groundObject = GameObject.FindGameObjectWithTag("Ground");

        // Ground�� Collider�� ����Ͽ� ��ġ ���� ���
        Collider groundCollider = groundObject.GetComponent<Collider>();
        Vector3 groundPosition = groundObject.transform.position;

        // Ground�� bounds ������ ������ x, z �� ����
        float randomX = Random.Range(groundCollider.bounds.min.x, groundCollider.bounds.max.x);
        float randomZ = Random.Range(groundCollider.bounds.min.z, groundCollider.bounds.max.z);

        // Raycast�� ����Ͽ� �ش� ��ġ�� ��Ȯ�� Y�� ã��
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(randomX, 100f, randomZ), Vector3.down, out hit))
        {
            // Raycast�� ���� Y���� ����Ͽ� ���� ��ġ�� ���� ���� ����Ʈ ����
            return new Vector3(randomX, hit.point.y, randomZ);
        }

        // ���� Raycast�� ������ ���, �⺻���� ���̸� ��ȯ (��� ��Ȳ)
        return new Vector3(randomX, groundPosition.y, randomZ);
    }

    // ������ ������ �� ���� ������ ��ȯ
    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }


}
