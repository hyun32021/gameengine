using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    public SoundManager soundManager; // SoundManager를 Inspector에서 할당

    private NavMeshAgent m_Agent = null;
    private GameObject _target = null;

    private Animator m_Animator = null;

    public GameObject expGem;

    public int HP = 5;

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            var playerCtrl = coll.gameObject.GetComponent<PlayerCtrl>();
            playerCtrl.playerHp--;
            Debug.Log("Player HP: " + playerCtrl.playerHp);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            //m_Animator.SetBool("getHit", true);
            HP--; // 총알 맞고 HP 감소
        }
    }

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();

        if (soundManager == null)
        {
            soundManager = FindObjectOfType<SoundManager>(); // 자동으로 SoundManager 찾기
        }
    }

    // Update is called once per frame
    void Update()
    {
        _target = GameObject.FindWithTag("Player");
        m_Agent.SetDestination(_target.transform.position);

        // 사망 처리
        if (HP <= 0)
        {
            dieMonster();
            if (soundManager != null) // soundManager가 null인지 확인
            {
                soundManager.PlayEnemyDeadSound();
            }
            else
            {
                Debug.LogWarning("SoundManager is not assigned!");
            }
        }
    }

    void dieMonster()
    {
        Instantiate(expGem, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
