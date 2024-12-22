using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Rigidbody rb;

    public WeaponData weaponData;  // WeaponData를 참조하여 공격 범위와 공격력 받기
    [SerializeField] private GameObject explosionPrefab; // 폭발 이펙트 프리팹

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            // 발사 방향으로 힘을 주어 총알을 발사
            rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody not found on bullet!");
        }

        // 일정 시간이 지나면 총알이 사라지도록 설정
        Destroy(gameObject, 3f);
    }

    // 충돌이 발생했을 때 폭발 효과를 실행
    void OnTriggerEnter(Collider coll)
    {
        // 적과 충돌했을 때만 실행
        if (coll.CompareTag("Monster") || coll.CompareTag("Boss"))
        {
            // 폭발 이펙트 생성
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // 폭발 구체의 크기 설정
            ExplosionEffect explosionEffect = explosion.GetComponent<ExplosionEffect>();
            if (explosionEffect != null)
            {
                explosionEffect.SetExplosionSize(weaponData.attackRange); // attackRange 값을 전달
            }

            // 범위 내 적들에게 피해를 주기
            ApplyDamage();

            // 발사체 삭제
            Destroy(gameObject);
        }
    }


    private void ApplyDamage()
    {
        float explosionRadius = weaponData.attackRange;

        // LayerMask targetLayer = LayerMask.GetMask("Monster", "Boss"); // 대상 레이어 설정 (추후 적용 가능)
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, explosionRadius/2f /*, targetLayer */);

        foreach (Collider hit in hitEnemies)
        {
            if (hit.CompareTag("Monster"))
            {
                var monster = hit.gameObject.GetComponentInParent<MonsterCtrl>();  // 부모에서 MonsterCtrl을 찾음

                if (monster == null)
                {
                    // 부모가 없을 때 최상위 오브젝트에서 직접 찾기
                    monster = hit.gameObject.GetComponent<MonsterCtrl>();
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
            else if (hit.CompareTag("Boss"))
            {
                var boss = hit.GetComponent<BossMonster>();
                if (boss != null)
                {
                    boss.takeDamage(weaponData.attackPower);  // WeaponData의 공격력으로 피해 주기
                }
            }
        }
    }
}