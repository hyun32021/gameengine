using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectExp : MonoBehaviour
{
    [SerializeField] private float expRange = 5f;  // ����ġ ������Ʈ�� ������ ����
    [SerializeField] private float expSpeed = 5f;  // ����ġ ������� �ӵ�

    private PlayerCtrl playerCtrl; // �÷��̾� ��Ʈ�� ��ũ��Ʈ ����
    private Collider triggerCollider; // �ڽ� ������Ʈ�� Collider

    // ����ġ ������Ʈ�� �����ϴ� ����Ʈ
    private List<Collider> experienceObjects = new List<Collider>();

    private void Start()
    {
        // �θ� ��ü�� PlayerCtrl ��ũ��Ʈ�� ã��
        playerCtrl = GetComponentInParent<PlayerCtrl>();

        // null üũ: PlayerCtrl�� ����� �������� �ʾҴٸ� ��� ���
        if (playerCtrl == null)
        {
            Debug.LogWarning("PlayerCtrl not found in parent objects.");
        }

        // �ڽ� ������Ʈ�� Collider�� ã�� �ʱ� ���� ����
        triggerCollider = GetComponent<Collider>();
        SetColliderRange(expRange); // �ʱ� ���� ����
    }

    void Update()
    {
        // ����ġ ������Ʈ�� ������� ȹ���ϴ� ����
        CollectExperience();
    }

    // ����ġ ������Ʈ�� �������, ���� ������ ȹ���ϴ� �޼���
    private void CollectExperience()
    {
        // ����Ʈ�� �ִ� ����ġ ������Ʈ�鸸 ó��
        for (int i = 0; i < experienceObjects.Count; i++)
        {
            Collider collider = experienceObjects[i];

            if (collider != null)  // null üũ �߰�
            {
                // ����ġ ������Ʈ�� �÷��̾� ������ ������ϴ�
                Vector3 direction = (transform.position - collider.transform.position).normalized;

                // �÷��̾ ���� ��� �̵��ϰԲ� ������ ����
                collider.transform.position = Vector3.MoveTowards(collider.transform.position, transform.position, expSpeed * Time.deltaTime);

                // ����ġ ������Ʈ�� �÷��̾ ��������� ����ġ�� �ø��ϴ�
                if (Vector3.Distance(transform.position, collider.transform.position) < 1f)
                {
                    // �÷��̾��� ����ġ�� ����
                    if (playerCtrl != null)  // null üũ �߰�
                    {
                        playerCtrl.AddExperience(1);  // 1��ŭ ����ġ�� �ø�
                    }

                    // ����Ʈ���� ����ġ ������Ʈ�� ����
                    experienceObjects.RemoveAt(i);

                    // ����ġ ������Ʈ �ı�
                    Destroy(collider.gameObject);  // ����ġ ������Ʈ ����
                    Debug.Log("Exp collected!");
                    continue;  // ���� ������Ʈ ���� ��, ���� ������Ʈ�� �̵�
                }
            }
        }
    }


    // �÷��̾�� ����ġ ������Ʈ�� �浹�ϸ� ����Ʈ�� �߰�
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Exp") && !experienceObjects.Contains(other))
        {
            experienceObjects.Add(other);  // ����ġ ������Ʈ�� ����Ʈ�� �߰�
        }
    }

    // �÷��̾�� ����ġ ������Ʈ�� �浹�� ������ ����Ʈ���� ����
    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.CompareTag("Exp"))
        {
            experienceObjects.Remove(other);  // ����ġ ������Ʈ�� ����Ʈ���� ����
        }
    }

    // �ݶ��̴��� ������ �������� �����ϴ� �޼���
    public void SetColliderRange(float range)
    {
        if (triggerCollider != null)
        {
            // `Collider`�� ������ �����ϱ� ���� `SphereCollider`�� ��쿡�� ����
            if (triggerCollider is SphereCollider sphereCollider)
            {
                sphereCollider.radius = range;
            }
            else
            {
                Debug.LogWarning("Collider is not a SphereCollider. Cannot change radius.");
            }
        }
    }
}
