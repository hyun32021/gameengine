using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss : BossMonster
{
    private Vector3 targetPos;
    private float distance;
    private float speed;
    private bool isJumping = false;
    private float jumpCooldownTimer = 0f;

    [SerializeField] private float minAttackRange = 10f;   // 최소 거리
    [SerializeField] private float maxAttackRange = 50f;  // 최대 거리
    [SerializeField] private float jumpCooldown = 10f;    // 점프 공격 쿨타임
    [SerializeField] private float attackRange = 9f;
    public MeshRenderer meshRenderer;
    private GameObject meshObject;

    [SerializeField] private ParticleSystem white;
    [SerializeField] private ParticleSystem brown;
    [SerializeField] private ParticleSystem green;


    [SerializeField] private GameObject rock;
    [SerializeField] private float rockCooldown = 3f;
    [SerializeField] private float rockCooldownTimer = 0f;
    [SerializeField] private Transform attackTr;

    public void Attack()
    {
         if (rock != null)
        {
            if(rockCooldownTimer <= 0f)
            {
                float distance = Vector3.Distance(transform.position, _target.transform.position);
                if (distance <= 10f)
                {
                    if (animator != null)
                    {
                        animator.SetTrigger("Attack");
                    }
                }
            }
            else
            {
                rockCooldownTimer -= Time.deltaTime;
            }
        }
    }
    public override void Attack1()
    {
        if (jumpCooldownTimer <= 0f)
        {
            distance = Vector3.Distance(transform.position, _target.transform.position);
            if (distance >= minAttackRange && distance <= maxAttackRange)
            {
                // 공격 시작
                targetPos = _target.transform.position; // 플레이어의 위치 기억
                speed = distance / 1.1f; // 점프 속도 계산 (애니메이션 길이에 맞게)
                animator.SetTrigger("Attack1"); // 점프 애니메이션 실행
                jumpCooldownTimer = jumpCooldown; // 쿨타임 초기화

                // 공격 범위 표시 활성화
                ShowAttackRange(targetPos);
            }
        }
        else
        {
            jumpCooldownTimer -= Time.deltaTime;
        }
    }

    void ShowAttackRange(Vector3 position)
    {
        if (meshObject == null && meshRenderer != null)
        {
            meshObject = Instantiate(meshRenderer.gameObject);
        }
        
        if (meshObject != null)
        {
            meshObject.SetActive(true);
            meshObject.transform.position = position;
            meshObject.transform.localScale = new Vector3(attackRange * 2f, 1f, attackRange * 2f);  // 크기 설정            
        }
    }

    void HideAttackRange()
    {
        if (meshRenderer != null)
        {
            meshObject.SetActive(false);
        }
    }
    void OnThrow()
    {
        var Rock = Instantiate(rock, attackTr.position, attackTr.rotation);
        Rock.SetActive(true);
        rockCooldownTimer = rockCooldown;
    }
    // 애니메이션 이벤트: 점프 대기
    void OnIdle()
    {
        b_Agent.isStopped = true; // 이동 정지
    }

    // 애니메이션 이벤트: 점프 시작
    void OnJump()
    {
        b_Agent.isStopped = false;
        isJumping = true;
    }

    // 애니메이션 이벤트: 내려찍기
    void OnSlam()
    {
        b_Agent.isStopped = true;
        HideAttackRange(); // 공격 범위 표시 비활성화

        white?.Play();
        brown?.Play();
        green?.Play();

        // 플레이어와의 충돌 감지
        Collider[] hitColliders = Physics.OverlapSphere(targetPos, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                var player = hitCollider.GetComponent<PlayerCtrl>();
                player?.TakeDamage(_damage); // 데미지 적용
            }
        }
    }
    // 애니메이션 이벤트: 점프 종료
    void OnJumpEnd()
    {
        b_Agent.isStopped = false;
        isJumping = false; // 점프 상태 종료
    }

    protected override void Update()
    {
        base.Update();
        Attack();
        // 점프 이동 처리
        if (isJumping)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }
}
