using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashScript : MonoBehaviour
{
    public WeaponData weaponData;  // WeaponData 타입을 참조 (SwordData 등)
    private Collider slashCollider;
    private ParticleSystem slashParticles;

    // Start is called before the first frame update
    void Start()
    {
        // 콜라이더와 파티클 시스템을 찾음
        slashCollider = GetComponent<Collider>();

        // 자식 오브젝트에서 파티클 시스템을 찾음
        slashParticles = GetComponentInChildren<ParticleSystem>();

        // 공격 범위에 비례하여 콜라이더 크기와 파티클 크기 설정
        SetAttackRange(weaponData.attackRange);

        // 일정 시간이 지나면 게임 오브젝트 삭제
        Destroy(gameObject, 0.95f);
    }

    // 공격 범위에 비례하여 콜라이더와 파티클 크기 설정
    void SetAttackRange(float range)
    {
        if (slashCollider != null)
        {
            // 콜라이더 크기 조정 (attackRange에 비례)
            if (slashCollider is BoxCollider boxCollider)
            {
                boxCollider.size = new Vector3(range, boxCollider.size.y, boxCollider.size.z);
            }
            else if (slashCollider is SphereCollider sphereCollider)
            {
                sphereCollider.radius = range;
            }
        }

        if (slashParticles != null)
        {
            // 파티클 크기 조정 (attackRange에 비례)
            var main = slashParticles.main;
            main.startSize = range;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Monster"))
        {
            var monster = coll.gameObject.GetComponentInParent<MonsterCtrl>();  // 부모에서 MonsterCtrl을 찾음

            if (monster == null)
            {
                // 부모가 없을 때 최상위 오브젝트에서 직접 찾기
                monster = coll.gameObject.GetComponent<MonsterCtrl>();
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
        else if (coll.CompareTag("Boss"))
        {
            var b_monster = coll.gameObject.GetComponent<BossMonster>();
            if (b_monster != null)
            {
                b_monster.takeDamage(weaponData.attackPower);
            }
        }
    }
}
