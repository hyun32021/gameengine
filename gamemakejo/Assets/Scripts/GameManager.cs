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
            Vector3 spawnPosition = RandomSpawnPoint();
            int monsterType = Random.Range(0, 2);
            if (monsterType == 0)
                Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            if (monsterType == 1)
                Instantiate(monsterPrefab2, spawnPosition, Quaternion.identity);
        }
    }
    void SpawnBoss()
    {
        Vector3 spawnPosition = RandomSpawnPoint();
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Boss Monster Spawned!");
    }
    // Ground 태그를 가진 오브젝트 위에 랜덤 스폰 포인트 생성
    Vector3 RandomSpawnPoint()
    {
        // "Ground" 태그를 가진 오브젝트를 찾음
        GameObject groundObject = GameObject.FindGameObjectWithTag("Ground");

        // Ground의 Collider를 사용하여 위치 범위 계산
        Collider groundCollider = groundObject.GetComponent<Collider>();

        // Ground의 bounds를 가져옴
        Vector3 minBounds = groundCollider.bounds.min;
        Vector3 maxBounds = groundCollider.bounds.max;

        float currentRangeReduction = 0.0f; // 범위 축소 값 초기화
        float maxRangeReduction = 0.5f; // 범위를 축소할 최대 값 (50% 축소 예제)

        int maxAttempts = 10; // 최대 재시도 횟수
        for (int i = 0; i < maxAttempts; i++)
        {
            // 현재 축소된 범위를 적용
            float randomX = Random.Range(
                Mathf.Lerp(minBounds.x, maxBounds.x, currentRangeReduction),
                Mathf.Lerp(maxBounds.x, minBounds.x, currentRangeReduction)
            );
            float randomZ = Random.Range(
                Mathf.Lerp(minBounds.z, maxBounds.z, currentRangeReduction),
                Mathf.Lerp(maxBounds.z, minBounds.z, currentRangeReduction)
            );

            // Raycast로 지면의 정확한 Y 좌표를 탐색
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(randomX, 100f, randomZ), Vector3.down, out hit))
            {
                // Raycast 성공, 정확한 스폰 지점 반환
                return new Vector3(randomX, hit.point.y, randomZ);
            }

            // Raycast 실패 시 범위를 점진적으로 줄임
            currentRangeReduction += maxRangeReduction / maxAttempts;
            Debug.LogWarning($"Raycast failed. Reducing range... Attempt {i + 1}/{maxAttempts}");
        }

        // 모든 시도 실패 시 예외 처리
        Debug.LogError("Failed to find a valid spawn point within the bounds.");
        throw new System.Exception("Unable to generate a valid spawn point!");
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
