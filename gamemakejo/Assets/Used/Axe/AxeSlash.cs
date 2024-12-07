using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSlash : MonoBehaviour
{
    public WeaponData Axe;
    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Monster"))
        {
            var monster = coll.gameObject.GetComponent<MonsterCtrl>();
            monster.HP -= Axe.attackPower;
        }
        else if (coll.CompareTag("Boss"))
        {
            var b_monster = coll.gameObject.GetComponent<BossMonster>();
            b_monster.HP -= Axe.attackPower;
        }
    } 
}
