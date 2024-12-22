using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MonsterCtrl : MonoBehaviour
{
    public SoundManager soundManager; // SoundManager를 Inspector에서 할당

    private NavMeshAgent m_Agent = null;
    private GameObject _target = null;

    private Animator m_Animator = null;

    public GameObject expGem;

    public float HP = 5f;
    public float currentHp;
    public float damage = 1f;
    private float attackRange = 2f;

    public delegate void MonsterDeathEventHandler();
    public static event MonsterDeathEventHandler OnMonsterDeath;
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            var playerCtrl = coll.gameObject.GetComponent<PlayerCtrl>();
            playerCtrl.TakeDamage(damage);
        }
    }

    void Start()
    {
        _target = GameObject.FindWithTag("Player");
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        HP = MonsterData.monsterHP;
        currentHp = HP;
        damage = MonsterData.monsterAttack;
        if (soundManager == null)
        {
            soundManager = FindObjectOfType<SoundManager>(); // 자동으로 SoundManager 찾기
        }
    }
    public void takeDamage(float damage)
    {
        currentHp -= damage;
        m_Animator.SetTrigger("GetHit");
    }
    // Update is called once per frame
    void Update()
    {

        // 플레이어와의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

        if (distanceToTarget <= attackRange)
        {
            // 공격 애니메이션 실행
            m_Animator.SetTrigger("Attack");
        }
        else
        {
            // 플레이어를 따라 이동
            m_Agent.isStopped = false; // 이동 가능
            m_Agent.SetDestination(_target.transform.position);
        }

        // 사망 처리
        if (currentHp <= 0)
        {
            Die();
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

    void Die()
    {
        Instantiate(expGem, transform.position, Quaternion.identity);

        OnMonsterDeath?.Invoke();

        Destroy(gameObject);  // 몬스터 오브젝트 파괴
    }
    void OnAttackStart()
    {
        m_Agent.isStopped = true; // 이동 정지
        Debug.Log("공격 시작!");
    }

    void OnAttackEnd()
    {
        m_Agent.isStopped = false; // 이동 재개
        Debug.Log("공격 종료!");
    }
}
