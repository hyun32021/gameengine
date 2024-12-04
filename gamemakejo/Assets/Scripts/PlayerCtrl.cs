using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private float v = 0;     //���� �Է��� ������ ���� (�յ� �̵�)
    private float h = 0;     //���� �Է��� ������ ���� (�¿� �̵�)

    private float v1 = 0;    //������ ���� �̵� ���� �����ϴ� ����
    private float h1 = 0;    //������ ���� �̵� ���� �����ϴ� ����

    private Vector3 moveDir = new Vector3(0, 0, 0); //�̵� ������ ��Ÿ���� ����

    [SerializeField] private float speed = 10f;   //ĳ������ �̵� �ӵ�
    [SerializeField] private float runSpeed = 20f;  //�޸��� �ӵ�
    [SerializeField] private float rotSpeed = 500f;  //ĳ������ ȸ�� �ӵ�

    private Animator _animator;

    public GameObject bullet;        // �Ѿ� Prefab
    public Transform fireTr;         // �Ѿ� �߻� ��ġ (�÷��̾� ��)

    public int playerHp = 5;
    [SerializeField] private int playerExp = 0;
    [SerializeField] private int maxExp = 10;
    private int playerLv = 1;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerExp >= maxExp)
        {
            LevelUP();
        }
        _animator.SetBool("p_Attack", false);
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        v1 = v * Mathf.Sqrt(1 - (Mathf.Pow(h, 2) / 2));
        h1 = h * Mathf.Sqrt(1 - (Mathf.Pow(v, 2) / 2));

        float currentSpeed = speed;

        if (v > 0 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            currentSpeed = runSpeed;
        }

        moveDir = (Vector3.forward * v1 + Vector3.right * h1);

        // ĳ���� �̵�
        transform.Translate(moveDir * currentSpeed * Time.deltaTime, Space.Self);
        // ȸ��
        transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));

        _animator.SetFloat("p_V", v1);
        _animator.SetFloat("p_H", h1);

        // �Ѿ� �߻� (���콺 ���� ��ư Ŭ��)
        if (Input.GetMouseButtonDown(0))
        {
            FireBullet();
            _animator.SetBool("p_Attack", true);
        }
        if (playerHp <= 0)
        {
            
        }
    }
    
    // �Ѿ� �߻� �޼���
    private void FireBullet()
    {
        if (bullet != null && fireTr != null)
        {
            // �Ѿ� �ν��Ͻ��� �߻� ��ġ���� ����
            Instantiate(bullet, fireTr.position, fireTr.rotation);
        }
        else
        {
            Debug.LogWarning("Bullet or Fire Transform is not set up!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exp"))
        {
            playerExp++;
            Destroy(other.gameObject);
            Debug.Log("Exp: " + playerExp);
        }
    }
    void LevelUP()
    {
        playerExp = 0;
        maxExp += 5;
        playerLv += 1;
        Debug.Log("Level UP");
    }
}

