using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    public GameObject bossPrefab; // 보스 몬스터

    [SerializeField] private int numberOfMonsters = 5; // 소환할 몬스터 수
    [SerializeField] private int maxMonsters = 50;  // 최대 몬스터 수

    [SerializeField] private float spawnInterval = 5f; // 몬스터 스폰 간격
    [SerializeField] private float spawnInterval_Boss; // 보스 스폰 간격
    [SerializeField] private float diffInterval = 30f; // 몬스터 강화 인터벌 변수


    [SerializeField] private float healthMultiplier = 1.1f;
    [SerializeField] private float damageMultiplier = 1.1f;

    private float spawnTimer = 0f; // 몬스터 스폰 타이머
    private float spawnTimer_Boss = 0f; // 보스 스폰 타이머
    private bool bossSpawned = false; // 보스 스폰 여부

    private float diffTimer = 0f; // 몬스터 강화 타이머 변수 추가

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
        //몬스터 강화 초기화
        if (player.GetComponent<PlayerCtrl>().playerHp<=0) 
        {
            MonsterCtrl prefab1Ctrl = monsterPrefab.GetComponent<MonsterCtrl>();
            MonsterCtrl prefab2Ctrl = monsterPrefab2.GetComponent<MonsterCtrl>();
            prefab1Ctrl.HP = MonsterData.monsterHP;
            prefab2Ctrl.HP = MonsterData.monsterHP;
            prefab1Ctrl.damage = MonsterData.monsterAttack;
            prefab2Ctrl.damage = MonsterData.monsterAttack;
        }
        // 타이머 증가
        spawnTimer += Time.deltaTime;
        spawnTimer_Boss += Time.deltaTime;
        diffTimer += Time.deltaTime; // 타이머 증가

        // 몬스터를 일정시간마다 강화
        if (diffTimer >= diffInterval)
        {
            diffTimer = 0f; // 타이머 초기화
            LevelUpMonsters(); // 몬스터 레벨업 실행
        }

        // 몬스터가 일정 시간 간격으로 스폰, 보스가 스폰되지 않았다면
        if (spawnTimer >= spawnInterval && !bossSpawned && GetCurrentMonsterCount() < maxMonsters)
        {
            SpawnMonsters();
            spawnTimer = 0f; // 타이머 초기화
        }

        // 보스 몬스터 스폰
        if (!bossSpawned)
        {
            if (spawnTimer_Boss >= spawnInterval_Boss)
            {
                SpawnBoss();
                bossSpawned = true; // 보스 스폰 여부 변경
            }
        }
    }

    // 몬스터를 스폰하는 메서드
    void SpawnMonsters()
    {
        for (int i = 0; i < numberOfMonsters; i++)
        {
            // 몬스터 수가 초과되지 않도록 체크
            if (GetCurrentMonsterCount() >= maxMonsters || bossSpawned)
                return;

            Vector3 spawnPosition = RandomSpawnPoint();
            int monsterType = Random.Range(0, 2);
            if (monsterType == 0)
                Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            if (monsterType == 1)
                Instantiate(monsterPrefab2, spawnPosition, Quaternion.identity);
        }
    }

    // 보스 몬스터를 스폰하는 메서드
    void SpawnBoss()
    {
        Vector3 spawnPosition = RandomSpawnPoint();
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Boss Monster Spawned!");
    }
    void LevelUpMonsters()
    {
        Debug.Log("Increasing monster difficulty!");

        // "Monster" 태그를 가진 모든 활성 몬스터에 대해 강화 적용
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            var monsterCtrl = monster.GetComponent<MonsterCtrl>();
            if (monsterCtrl != null)
            {
                monsterCtrl.HP *= healthMultiplier;
                if (monsterCtrl.currentHp != monsterCtrl.HP)
                {
                    float healthRatio = monsterCtrl.currentHp / monsterCtrl.HP;
                    monsterCtrl.currentHp = monsterCtrl.HP * healthMultiplier;  // 몬스터가 강화돼도 체력의 비율을 유지하게 변경
                }
                else monsterCtrl.currentHp *= healthMultiplier;
                monsterCtrl.damage *= damageMultiplier;
            }
        }

        // 보스 몬스터 강화
        GameObject boss = GameObject.FindWithTag("Boss");
        if (boss != null)
        {
            var bossCtrl = boss.GetComponent<BossMonster>();
            if (bossCtrl != null)
            {
                bossCtrl.HP *= healthMultiplier;
                if (bossCtrl.currentHp != bossCtrl.HP)
                {
                    float healthRatio = bossCtrl.currentHp / bossCtrl.HP;
                    bossCtrl.currentHp = bossCtrl.HP * healthMultiplier;  // 몬스터가 강화돼도 체력의 비율을 유지하게 변경
                }
                else bossCtrl.currentHp *= healthMultiplier;
                bossCtrl._damage *= damageMultiplier;
            }
        }

        // 몬스터 프리팹 강화
        MonsterCtrl prefab1Ctrl = monsterPrefab.GetComponent<MonsterCtrl>();
        MonsterCtrl prefab2Ctrl = monsterPrefab2.GetComponent<MonsterCtrl>();
        if (prefab1Ctrl != null)
        {
            prefab1Ctrl.HP *= healthMultiplier;
            prefab1Ctrl.damage *= damageMultiplier;
        }
        if (prefab2Ctrl != null)
        {
            prefab2Ctrl.HP *= healthMultiplier;
            prefab2Ctrl.damage *= damageMultiplier;
        }
    }


    // 현재 활성화된 몬스터의 수를 반환
    int GetCurrentMonsterCount()
    {
        // "Monster" 태그를 가진 게임 오브젝트의 수를 확인
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        return monsters.Length;
    }

    // "Ground" 태그를 가진 오브젝트 위에 랜덤 스폰 포인트 생성
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

    // 보스 몬스터 처치 후  씬 로드
    public void LoadNextScene()
    {
        Time.timeScale = 1f;

        // 현재 씬을 가져옴
        Scene currentScene = SceneManager.GetActiveScene();

        // 현재 씬 이름으로 다음 씬의 인덱스를 계산
        int currentIndex = currentScene.buildIndex;
        int nextSceneIndex = (currentIndex + 1) % SceneManager.sceneCountInBuildSettings; // 마지막 씬 이후 첫 씬으로 돌아가게 설정

        // 다음 씬 로드
        SceneManager.LoadScene(nextSceneIndex);
    }


}

