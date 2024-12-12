using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class BossMonster : MonoBehaviour
{
    protected NavMeshAgent b_Agent;
    protected GameObject _target;
    protected Animator animator;

    public float _damage = 5;
    public float HP = 30;
    public float currentHp;

    // 공격 타입을 각 보스에서 정의하게 하므로 기본 메서드는 abstract로 처리
    public abstract void Attack1();

    private void Start()
    {
        b_Agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        _target = GameObject.FindWithTag("Player");
        currentHp = HP;
    }

    protected virtual void Update()
    {
        b_Agent.SetDestination(_target.transform.position);

        if (currentHp <= 0)
        {
            currentHp = 0;
            OnBossDefeated();
        }
        // 보스의 공격 메서드를 호출 (각 보스에 맞는 공격을 오버라이드)
        Attack1();
    }
    
    public void takeDamage(float damage)
    {
        currentHp -= damage;
        animator.SetTrigger("GetHit");
    }

    private void OnBossDefeated()
    {
        // 게임 진행 상황에 맞게 다음 씬을 로드
        GameManager.Instance.LoadNextScene();
        Destroy(gameObject);
    }
}
