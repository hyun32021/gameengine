using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string lobbyScene = "Lobby"; // 로비 씬
    public string stage1Scene = "Stage1"; // Stage1 씬
    public string stage2Scene = "Stage2"; // Stage2 씬
    public string stage3Scene = "Stage3"; // Stage3 씬
    public string gameOverScene = "GameOver"; // 게임 오버 씬
    public string clearScene = "Clear"; // 클리어 씬

    public GameObject player;
    public GameObject monsterPrefab;
    public GameObject monsterPrefab2;
    public GameObject bossPrefab; // ���� ���� ������

    public int numberOfMonsters = 5; // �� ���� ������ ���� ��
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 20f;
    public float spawnInterval = 5f; // ���� ���� �ֱ�
    public float spawnInterval_Boss = 300f;

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
    void Start()
    {
        
    }

    // Update is called once per frame
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
            spawnTimer_Boss += Time.deltaTime;

            if (spawnTimer_Boss >= spawnInterval_Boss)
            {
                SpawnBoss();
                bossSpawned = true; // ������ �� ���� �����ǵ��� ����
            }
        }
    }
    // ���� ���� ����Ʈ ���� �� ���� ����
    void SpawnMonsters()
    {
        for (int i = 0; i < numberOfMonsters; i++)
        {
            Vector3 spawnPosition = GenerateRandomSpawnPoint();
            int monsterType = Random.Range(0, 2);
            if (monsterType == 0)
                Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            if (monsterType == 1)
                Instantiate(monsterPrefab2, spawnPosition, Quaternion.identity);
        }
    }
    void SpawnBoss()
    {
        Vector3 spawnPosition = GenerateRandomSpawnPoint();
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Boss Monster Spawned!");
    }
    // �÷��̾�κ��� ���� �Ÿ� ������ ���� ���� ����Ʈ ����
    Vector3 GenerateRandomSpawnPoint()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        return player.transform.position + new Vector3(randomDirection.x, 0, randomDirection.y) * randomDistance;
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
