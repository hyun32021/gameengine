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
    public GameObject bossPrefab; // 보스 몬스터 프리팹

    public int numberOfMonsters = 5; // 한 번에 생성할 몬스터 수
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 20f;
    public float spawnInterval = 5f; // 몬스터 생성 주기
    public float spawnInterval_Boss = 300f; // 보스 몬스터 출현 주기

    private float spawnTimer = 0f; // 경과 시간 추적
    private float spawnTimer_Boss = 0f;//보스 몬스터 출현시간
    private bool bossSpawned = false; // 보스 생성 여부 확인

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
        // 경과 시간을 추적하고 spawnInterval에 도달하면 몬스터 소환
        spawnTimer += Time.deltaTime;
        spawnTimer_Boss += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnMonsters();
            spawnTimer = 0f; // 타이머 초기화
        }
        if (!bossSpawned)
        {
            if (spawnTimer_Boss >= spawnInterval_Boss)
            {
                SpawnBoss();
                bossSpawned = true; // 보스가 한 번만 생성되도록 설정
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
            bossSpawned = true; // 보스가 한 번만 생성되도록 설정
            Debug.Log("Boss Monster Spawned!");
        }
    }

    // Ground 태그를 가진 오브젝트 위에 랜덤 스폰 포인트 생성
    Vector3 GenerateRandomGroundSpawnPoint()
    {
        // "Ground" 태그를 가진 오브젝트를 찾음
        GameObject groundObject = GameObject.FindGameObjectWithTag("Ground");

        // Ground의 Collider를 사용하여 위치 범위 계산
        Collider groundCollider = groundObject.GetComponent<Collider>();
        Vector3 groundPosition = groundObject.transform.position;

        // Ground의 bounds 내에서 랜덤한 x, z 값 생성
        float randomX = Random.Range(groundCollider.bounds.min.x, groundCollider.bounds.max.x);
        float randomZ = Random.Range(groundCollider.bounds.min.z, groundCollider.bounds.max.z);

        // Raycast를 사용하여 해당 위치의 정확한 Y값 찾기
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(randomX, 100f, randomZ), Vector3.down, out hit))
        {
            // Raycast로 얻은 Y값을 사용하여 지면 위치에 맞춘 스폰 포인트 생성
            return new Vector3(randomX, hit.point.y, randomZ);
        }

        // 만약 Raycast가 실패할 경우, 기본적인 높이를 반환 (비상 상황)
        return new Vector3(randomX, groundPosition.y, randomZ);
    }

    // 조건을 만족할 때 다음 씬으로 전환
    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }


}
