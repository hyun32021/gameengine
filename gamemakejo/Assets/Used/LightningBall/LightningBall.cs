using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : MonoBehaviour
{
    public float searchRadius = 10f;  // 적을 찾을 범위
    private Transform closestEnemy;    // 가장 가까운 적의 Transform
    private List<Transform> previousEnemies = new List<Transform>();  // 이전에 락온했던 적들의 목록
    private int maxChainCount = 3;  // 최대 연쇄 횟수
    public float moveSpeed = 30f;  // 이동 속도
    private Rigidbody rb;  // Rigidbody 컴포넌트
    [SerializeField] private int currentChainCount = 0;  // 현재 연쇄 횟수
    public WeaponData weaponData;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody 컴포넌트를 가져옵니다.

        // weaponData가 LightningballData인 경우에만 연쇄 횟수를 받아와서 최대 연쇄 횟수에 초기화
        if (weaponData is LightningballData lightningballData)
        {
            maxChainCount = lightningballData.chainCount;  // LightningballData에서 연쇄 횟수 값 받아오기
        }
        searchRadius = weaponData.attackRange;
        // 몬스터의 죽음 이벤트를 구독
        MonsterCtrl.OnMonsterDeath += CheckMonsters;
    }

    void OnDestroy()
    {
        // 구독 해제
        MonsterCtrl.OnMonsterDeath -= CheckMonsters;
    }

    void Update()
    {
        // 타깃이 설정되지 않았다면, 가장 가까운 적을 찾는다
        if (closestEnemy == null || !closestEnemy.gameObject.activeInHierarchy)
        {
            // 타깃이 사라졌거나 죽었으면, 새로운 타깃을 찾는다
            FindClosestEnemy();
        }

        // 타깃이 설정되어 있다면, 그 타깃을 추적한다
        if (closestEnemy != null)
        {
            MoveTowardsEnemy();
        }
    }

    void FindClosestEnemy()
    {
        // "Monster"와 "Boss" 태그를 가진 적을 찾기
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        float minDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (GameObject monster in monsters)
        {
            // 이미 타격한 몬스터는 제외
            if (previousEnemies.Contains(monster.transform))
                continue;

            float distance = Vector3.Distance(transform.position, monster.transform.position);

            // searchRadius 내의 몬스터만 고려
            if (distance <= searchRadius && distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = monster.transform;
            }
        }

        // 보스가 있을 경우 보스도 거리 비교
        if (boss != null && !previousEnemies.Contains(boss.transform))
        {
            float bossDistance = Vector3.Distance(transform.position, boss.transform.position);
            if (bossDistance <= searchRadius && bossDistance < minDistance)
            {
                minDistance = bossDistance;
                nearestEnemy = boss.transform;
            }
        }

        // 가장 가까운 적을 찾으면, 그것을 락온한다
        if (nearestEnemy != null)
        {
            closestEnemy = nearestEnemy;
            Debug.Log("가장 가까운 적 락온: " + closestEnemy.name);
        }
        else
        {
            // 유효한 타깃이 없고 연쇄 횟수가 남아 있다면 프리팹을 파괴
            if (currentChainCount < maxChainCount)
            {
                Destroy(gameObject);
                Debug.Log("유효한 타깃이 없으므로 LightningBall 프리팹을 파괴합니다.");
            }
            else
            {
                Destroy(gameObject);
                Debug.Log("유효한 타깃이 없습니다. 그러나 연쇄가 종료되었습니다.");
            }
        }
    }

    void MoveTowardsEnemy()
    {
        // 만약 closestEnemy가 null이면, 더 이상 추적할 적이 없으므로 함수를 종료
        if (closestEnemy == null)
            return;

        // 락온된 적을 향해 이동
        Vector3 direction = (closestEnemy.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;  // Rigidbody를 이용해 물리적으로 이동
    }

    void OnTriggerEnter(Collider other)
    {
        // closestEnemy가 null이 아니고 "Monster" 또는 "Boss" 태그를 가진 적과 충돌 시, 현재 락온된 적과만 비교
        if (closestEnemy != null && (other.CompareTag("Monster") || other.CompareTag("Boss")) && other.transform == closestEnemy)
        {
            // 충돌한 적에 대한 추적 중지
            Debug.Log(closestEnemy.name + "에게 닿았습니다. 추적을 중지합니다.");

            // 피해를 입힘
            if (other.CompareTag("Monster"))
            {
                var monster = closestEnemy.gameObject.GetComponentInParent<MonsterCtrl>();  // 부모에서 MonsterCtrl을 찾음

                if (monster == null)
                {
                    // 부모가 없을 때 최상위 오브젝트에서 직접 찾기
                    monster = closestEnemy.gameObject.GetComponent<MonsterCtrl>();
                }

                if (monster != null)
                {
                    monster.takeDamage(weaponData.attackPower);
                }
                else
                {
                    Debug.Log("MonsterCtrl not found");
                }
            }
            else if (other.CompareTag("Boss"))
            {
                BossMonster bossCtrl = closestEnemy.GetComponent<BossMonster>();
                if (bossCtrl != null)
                {
                    bossCtrl.takeDamage(weaponData.attackPower);
                    Debug.Log(closestEnemy.name + "에게 피해를 입혔습니다.");
                }
            }

            // 첫 번째 타깃을 기억
            previousEnemies.Add(closestEnemy);  // 이번 타깃을 기록
            closestEnemy = null;  // 더 이상 추적할 적이 없으므로 null로 설정

            // 연쇄 횟수 체크
            currentChainCount++;

            // 최대 연쇄 횟수에 도달했다면, 더 이상 타깃을 찾지 않는다
            if (currentChainCount >= maxChainCount)
            {
                Debug.Log("최대 연쇄 횟수에 도달했습니다.");
                Destroy(gameObject);
            }

            // 새로운 타깃을 찾는다
            FindClosestEnemy();
        }
    }

    // 몬스터가 죽을 때마다 남은 몬스터를 확인하고, 모든 몬스터가 죽었으면 파괴
    void CheckMonsters()
    {
        // "Monster" 태그를 가진 오브젝트를 찾는다
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        // 몬스터와 보스가 없으면 프리팹을 파괴
        if ((monsters.Length == 0 && boss == null) || currentChainCount >= maxChainCount)
        {
            Destroy(gameObject);  // 현재 객체(프리팹)를 파괴
            Debug.Log("몬스터와 보스가 없거나 최대 연쇄 횟수에 도달했으므로 LightningBall 프리팹을 파괴합니다.");
        }
    }
}
