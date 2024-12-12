using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Monster : MonoBehaviour
{
    private MonsterCtrl m_MonsterCtrl;
    private SphereCollider sphereCollider;
    // Start is called before the first frame update
    void Start()
    {
        m_MonsterCtrl = GetComponent<MonsterCtrl>();
        sphereCollider = GetComponentInChildren<SphereCollider>();
        sphereCollider.enabled = false;
    }
    void OnStartAttack()
    {
        sphereCollider.enabled = true;
    }
    void OnEndAttack()
    {
        sphereCollider.enabled = false;
    }
   
}
