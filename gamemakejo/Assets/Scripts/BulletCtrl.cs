using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    Rigidbody rb;
    public WeaponData weaponData;  // WeaponData 타입을 참조
    private int pen;  // 관통력 변수

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // weaponData가 ArrowData인 경우에만 관통력을 받아와서 pen에 초기화
        if (weaponData is ArrowData arrowData)
        {
            pen = arrowData.penetration;  // ArrowData에서 관통력 값 받아오기
        }

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

    // 충돌 처리
    void OnTriggerEnter(Collider coll)
    {
        // 첫 번째 충돌에서 피해를 주고 관통
        if (coll.CompareTag("Monster"))
        {
        var monster = coll.gameObject.GetComponentInParent<MonsterCtrl>();  // 부모에서 MonsterCtrl을 찾음

        if (monster == null)
        {
            // 부모가 없을 때 최상위 오브젝트에서 직접 찾기
            monster = coll.gameObject.GetComponent<MonsterCtrl>();
            // pen이 0 이하이면 발사체 삭제
            if (pen <= 0)
            {
                Destroy(gameObject);  // pen이 0 이하일 때 발사체 삭제
            }
            else
            {
                pen--;  // 관통 후 pen을 1 감소시켜 관통력을 차감
            }
        }

        if (monster != null)
        {
            monster.takeDamage(weaponData.attackPower);
            // pen이 0 이하이면 발사체 삭제
            if (pen <= 0)
            {
                Destroy(gameObject);  // pen이 0 이하일 때 발사체 삭제
            }
            else
            {
                pen--;  // 관통 후 pen을 1 감소시켜 관통력을 차감
            }
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

                // pen이 0 이하이면 발사체 삭제
                if (pen <= 0)
                {
                    Destroy(gameObject);  // pen이 0 이하일 때 발사체 삭제
                }
                else
                {
                    pen--;  // 관통 후 pen을 1 감소시켜 관통력을 차감
                }
            }
        }
    }
}
