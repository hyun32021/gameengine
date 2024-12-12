using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    private Rigidbody rb;
    private Transform playerTr;
    private Vector3 targetPos;
    [SerializeField] private float speed = 5f;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            var playerCtrl = coll.gameObject.GetComponent<PlayerCtrl>();
            var bossMonster = GameObject.FindWithTag("Boss").gameObject.GetComponent<BossMonster>();
            playerCtrl.TakeDamage(bossMonster._damage);
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        playerTr = GameObject.FindWithTag("Player").transform;
        targetPos = playerTr.position;
        if (rb != null) 
        {
            rb.AddForce((targetPos-transform.position).normalized * speed, ForceMode.Impulse);
        }
        Destroy(gameObject, 3f);
    }
}
