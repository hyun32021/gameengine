using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSlash : MonoBehaviour
{
    public WeaponData weaponData;
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
            b_monster.takeDamage(weaponData.attackPower);
        }
    } 
}
