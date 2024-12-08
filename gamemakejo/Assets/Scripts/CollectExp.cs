using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectExp : MonoBehaviour
{
    [SerializeField] private float expRange = 5f;  // 경험치 오브젝트를 끌어당길 범위
    [SerializeField] private float expSpeed = 5f;  // 경험치 끌어당기는 속도

    private PlayerCtrl playerCtrl; // 플레이어 컨트롤 스크립트 참조
    private Collider triggerCollider; // 자식 오브젝트의 Collider

    // 경험치 오브젝트를 관리하는 리스트
    private List<Collider> experienceObjects = new List<Collider>();

    private void Start()
    {
        // 부모 객체의 PlayerCtrl 스크립트를 찾음
        playerCtrl = GetComponentInParent<PlayerCtrl>();

        // null 체크: PlayerCtrl이 제대로 설정되지 않았다면 경고를 출력
        if (playerCtrl == null)
        {
            Debug.LogWarning("PlayerCtrl not found in parent objects.");
        }

        // 자식 오브젝트의 Collider를 찾고 초기 범위 설정
        triggerCollider = GetComponent<Collider>();
        SetColliderRange(expRange); // 초기 범위 설정
    }

    void Update()
    {
        // 경험치 오브젝트를 끌어당기고 획득하는 로직
        CollectExperience();
    }

    // 경험치 오브젝트를 끌어당기고, 범위 내에서 획득하는 메서드
    private void CollectExperience()
    {
        // 리스트에 있는 경험치 오브젝트들만 처리
        for (int i = 0; i < experienceObjects.Count; i++)
        {
            Collider collider = experienceObjects[i];

            if (collider != null)  // null 체크 추가
            {
                // 경험치 오브젝트를 플레이어 쪽으로 끌어당깁니다
                Vector3 direction = (transform.position - collider.transform.position).normalized;

                // 플레이어를 향해 계속 이동하게끔 강제로 적용
                collider.transform.position = Vector3.MoveTowards(collider.transform.position, transform.position, expSpeed * Time.deltaTime);

                // 경험치 오브젝트가 플레이어에 가까워지면 경험치를 올립니다
                if (Vector3.Distance(transform.position, collider.transform.position) < 1f)
                {
                    // 플레이어의 경험치를 증가
                    if (playerCtrl != null)  // null 체크 추가
                    {
                        playerCtrl.AddExperience(1);  // 1만큼 경험치를 올림
                    }

                    // 리스트에서 경험치 오브젝트를 제거
                    experienceObjects.RemoveAt(i);

                    // 경험치 오브젝트 파괴
                    Destroy(collider.gameObject);  // 경험치 오브젝트 제거
                    Debug.Log("Exp collected!");
                    continue;  // 현재 오브젝트 제거 후, 다음 오브젝트로 이동
                }
            }
        }
    }


    // 플레이어와 경험치 오브젝트가 충돌하면 리스트에 추가
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Exp") && !experienceObjects.Contains(other))
        {
            experienceObjects.Add(other);  // 경험치 오브젝트를 리스트에 추가
        }
    }

    // 플레이어와 경험치 오브젝트가 충돌을 끝내면 리스트에서 제거
    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.CompareTag("Exp"))
        {
            experienceObjects.Remove(other);  // 경험치 오브젝트를 리스트에서 제거
        }
    }

    // 콜라이더의 범위를 동적으로 변경하는 메서드
    public void SetColliderRange(float range)
    {
        if (triggerCollider != null)
        {
            // `Collider`의 범위를 설정하기 위해 `SphereCollider`인 경우에만 동작
            if (triggerCollider is SphereCollider sphereCollider)
            {
                sphereCollider.radius = range;
            }
            else
            {
                Debug.LogWarning("Collider is not a SphereCollider. Cannot change radius.");
            }
        }
    }
}
