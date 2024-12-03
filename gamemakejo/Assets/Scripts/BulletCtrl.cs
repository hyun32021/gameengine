using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            // �߻� �������� ���� �־� �Ѿ��� �߻�
            rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody not found on bullet!");
        }

        // ���� �ð��� ������ �Ѿ��� ��������� ����
        Destroy(gameObject, 3f);
    }

    // �浹 ó��
    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Monster"))
        {
            var monster = coll.gameObject.GetComponent<MonsterCtrl>();
            monster.HP--;
            Destroy(gameObject);       // �Ѿ��� �ı�
        }
        else if (coll.CompareTag("Boss"))
        {
            var b_monster = coll.gameObject.GetComponent<BossMonster>();
            b_monster.HP--;
            Destroy(gameObject);       // �Ѿ��� �ı�
        }
    }
}


