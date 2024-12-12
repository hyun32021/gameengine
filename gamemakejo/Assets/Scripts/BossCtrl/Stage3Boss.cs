using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Boss : BossMonster
{
    [SerializeField] private GameObject iceMagic;
    public override void Attack1()
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerCtrl = other.gameObject.GetComponent<PlayerCtrl>();
            playerCtrl?.TakeDamage(_damage);
        }
    }
    void OnLichMagic()
    {
        b_Agent.isStopped = true;
        iceMagic?.SetActive(true);
    }
    void OffLichMagic()
    {
        b_Agent.isStopped = false;
        iceMagic?.SetActive(false);
    }
}
