using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Boss : BossMonster
{
    [SerializeField] private GameObject rock;
    [SerializeField] private float rockCooldown = 3f;
    [SerializeField] private float rockCooldownTimer = 0f;
    [SerializeField] private Transform attackTr;
    public override void Attack1()
    {
        if (rock != null)
        {
            if(rockCooldownTimer <= 0f)
            {
                float distance = Vector3.Distance(transform.position, _target.transform.position);
                if (distance <= 30f)
                {
                    var Rock = Instantiate(rock, attackTr.position, attackTr.rotation);
                    Rock.SetActive(true);
                    rockCooldownTimer = rockCooldown;
                }
            }
            else
            {
                rockCooldownTimer -= Time.deltaTime;
            }
        }
        
    }

    
}
