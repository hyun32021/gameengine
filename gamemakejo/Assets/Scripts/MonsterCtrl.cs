using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    public SoundManager soundManager; // SoundManager�� Inspector���� �Ҵ�

    private NavMeshAgent m_Agent = null;
    private GameObject _target = null;

    private Animator m_Animator = null;

    public GameObject expGem;

    public float HP = 5f;
    public float damage = 1f;

    public delegate void MonsterDeathEventHandler();
    public static event MonsterDeathEventHandler OnMonsterDeath;
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            var playerCtrl = coll.gameObject.GetComponent<PlayerCtrl>();
            playerCtrl.playerHp -= damage;
            Debug.Log("Player HP: " + playerCtrl.playerHp);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            //m_Animator.SetBool("getHit", true);
            //HP--; // �Ѿ� �°� HP ����
        }
    }

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        HP = MonsterData.monsterHP;
        damage = MonsterData.monsterAttack;
        if (soundManager == null)
        {
            soundManager = FindObjectOfType<SoundManager>(); // �ڵ����� SoundManager ã��
        }
    }

    // Update is called once per frame
    void Update()
    {
        _target = GameObject.FindWithTag("Player");
        m_Agent.SetDestination(_target.transform.position);

        // ��� ó��
        if (HP <= 0)
        {
            Die();
            if (soundManager != null) // soundManager�� null���� Ȯ��
            {
                soundManager.PlayEnemyDeadSound();
            }
            else
            {
                Debug.LogWarning("SoundManager is not assigned!");
            }
        }
    }

    void Die()
    {
        Instantiate(expGem, transform.position, Quaternion.identity);

        OnMonsterDeath?.Invoke();

        Destroy(gameObject);  // ���� ������Ʈ �ı�
    }
}
