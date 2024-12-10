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
            var monster = coll.gameObject.GetComponent<MonsterCtrl>();
            monster.takeDamage(weaponData.attackPower);
        }
        else if (coll.CompareTag("Boss"))
        {
            var b_monster = coll.gameObject.GetComponent<BossMonster>();
            b_monster.takeDamage(weaponData.attackPower);
        }
    } 
}
