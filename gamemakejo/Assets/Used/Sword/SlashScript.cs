using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.95f);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Monster"))
        {
            var monster = coll.gameObject.GetComponent<MonsterCtrl>();
            monster.HP--;
        }
        else if (coll.CompareTag("Boss"))
        {
            var b_monster = coll.gameObject.GetComponent<BossMonster>();
            b_monster.HP--;
        }
    }
}
