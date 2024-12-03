using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{
    public GameObject[] naturalPrefabs;  // 나무, 풀 등 여러 종류의 프리팹
    public int numberOfSpawns = 10;      // 소환할 프리팹 개수
    public float spawnRadius = 50f;      // 프리팹을 소환할 반경
    public Transform parentObject;       // 소환된 오브젝트의 부모 객체

    void Start()
    {
        SpawnNaturalObjects();
    }

    void SpawnNaturalObjects()
    {
        for (int i = 0; i < numberOfSpawns; i++)
        {
            Vector3 randomPosition = GetRandomPositionOnGround();

            if (randomPosition != Vector3.zero)
            {
                // 랜덤하게 선택한 자연물 프리팹을 소환하고 부모를 지정
                GameObject prefabToSpawn = naturalPrefabs[Random.Range(0, naturalPrefabs.Length)];
                GameObject spawnedPrefab = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
                spawnedPrefab.transform.SetParent(parentObject); // 부모 객체로 설정
            }
        }
    }

    // Ground 태그를 가진 오브젝트 위에서 랜덤 위치 계산
    Vector3 GetRandomPositionOnGround()
    {
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);
        Vector3 randomPosition = new Vector3(randomX, 0, randomZ);

        RaycastHit hit;
        // Raycast로 해당 위치에서 "Ground" 태그를 가진 오브젝트 확인
        if (Physics.Raycast(randomPosition + Vector3.up * 1000, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Ground"))  // Ground 태그를 가진 오브젝트만 체크
            {
                return hit.point; // Ground 위의 정확한 위치 반환
            }
        }

        return Vector3.zero; // "Ground" 태그가 아닌 오브젝트 위에는 소환하지 않음
    }
}
