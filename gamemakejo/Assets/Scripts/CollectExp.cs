using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectExp : MonoBehaviour
{
    [SerializeField] private float expRange = 5f;  // 경험치 오브젝트를 끌어당길 범위
    [SerializeField] private float expSpeed = 5f;  // 경험치 끌어당기는 속도

    private PlayerCtrl playerCtrl; // 플레이어 컨트롤 스크립트 참조
    private Collider triggerCollider; // 자식 오브젝트의 Collider
    private SoundManager soundManager; // 사운드 매니저 참조

    // 경험치 오브젝트를 관리하는 리스트
    private List<Collider> experienceObjects = new List<Collider>();

    private void Start()
    {
        // 부모 객체의 PlayerCtrl 스크립트를 찾음
        playerCtrl = GetComponentInParent<PlayerCtrl>();
        soundManager = FindObjectOfType<SoundManager>();

        // null 체크
        if (playerCtrl == null)
        {
            Debug.LogWarning("PlayerCtrl not found in parent objects.");
        }

        if (soundManager == null)
        {
            Debug.LogWarning("SoundManager not found in the scene.");
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

    private void CollectExperience()
    {
        for (int i = 0; i < experienceObjects.Count; i++)
        {
            Collider collider = experienceObjects[i];

            if (collider != null)
            {
                Vector3 direction = (transform.position - collider.transform.position).normalized;
                collider.transform.position = Vector3.MoveTowards(collider.transform.position, transform.position, expSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, collider.transform.position) < 1f)
                {
                    if (playerCtrl != null)
                    {
                        playerCtrl.AddExperience(1);
                    }

                    // 사운드 매니저를 통해 사운드 재생
                    if (soundManager != null)
                    {
                        soundManager.PlayExpCollectSound();
                    }

                    experienceObjects.RemoveAt(i);
                    Destroy(collider.gameObject);
                    Debug.Log("Exp collected!");
                    continue;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Exp") && !experienceObjects.Contains(other))
        {
            experienceObjects.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.CompareTag("Exp"))
        {
            experienceObjects.Remove(other);
        }
    }

    public void SetColliderRange(float range)
    {
        if (triggerCollider != null)
        {
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
