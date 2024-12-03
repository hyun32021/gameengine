using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target;         // ������ Ÿ�� (�÷��̾�)
    [SerializeField] private float distance = 10f;  // ī�޶�� �÷��̾� �� �Ÿ�
    [SerializeField] private float height = 5f;    // ī�޶��� ����
    [SerializeField] private float leftOffsetValue = -2f; // ī�޶� �÷��̾� �������� �̵��ϴ� �Ÿ�

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = Camera.main.transform;  // �⺻���� ī�޶� �ַ� ���� Ÿ���� ����
        }

        Cursor.lockState = CursorLockMode.Locked;  // Ŀ�� ���
        Cursor.visible = false;  // Ŀ�� �����
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        // ī�޶��� ��ǥ ��ġ ���: �÷��̾� ���� + ���� + ����
        Vector3 leftOffset = -target.right * leftOffsetValue;  // �������� �̵��ϴ� �Ÿ�
        Vector3 heightOffset = Vector3.up * height; // �÷��̾� ���� ������ ����

        // ī�޶�� �÷��̾��� ���ʰ� ����, ���ʿ� ��ġ�ϰ� ��
        Vector3 targetPosition = target.position - target.forward * distance + leftOffset + heightOffset;

        // ī�޶�� �÷��̾ �ٶ󺸴� ������ ���󰡾� �� (ȸ��)
        transform.position = targetPosition;

        // ī�޶�� �÷��̾�� ���� �������� �ٶ󺸰� ���� (ȸ������ �÷��̾�� �����ϰ� ����)
        transform.rotation = Quaternion.Euler(0f, target.eulerAngles.y, 0f);  // �÷��̾��� y ȸ������ ���󰡸� ī�޶�� �����ϰ� ȸ��
    }
}

