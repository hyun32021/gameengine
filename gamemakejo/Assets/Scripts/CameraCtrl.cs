using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target;         // 추적할 타겟 (플레이어)
    [SerializeField] private float distance = 10f;  // 카메라와 플레이어 간 거리
    [SerializeField] private float height = 5f;     // 카메라의 높이
    [SerializeField] private float leftOffsetValue = -2f; // 카메라가 플레이어 왼쪽으로 이동하는 거리

    private bool isCursorLocked = true;  // 커서 잠금 상태 확인용

    void Start()
    {
        // 커서를 기본 잠금 상태로 설정
        LockCursor();
    }

    void Update()
    {
        // 타겟을 따라가는 카메라 동작
        FollowPlayer();
    }

    /// <summary>
    /// 카메라가 플레이어를 추적하는 로직
    /// </summary>
    void FollowPlayer()
    {
        if (target == null) return;  // 타겟이 설정되지 않은 경우 실행하지 않음

        // 카메라의 목표 위치 계산: 플레이어 뒤쪽 + 왼쪽 + 위쪽
        Vector3 leftOffset = -target.right * leftOffsetValue;  // 왼쪽으로 이동하는 거리
        Vector3 heightOffset = Vector3.up * height;           // 플레이어 위로 설정된 높이
        Vector3 targetPosition = target.position - target.forward * distance + leftOffset + heightOffset;

        // 카메라의 위치와 회전값 설정
        transform.position = targetPosition;
        transform.rotation = Quaternion.Euler(0f, target.eulerAngles.y, 0f);  // 플레이어의 y 회전값과 동일하게 설정
    }

    /// <summary>
    /// 커서를 잠그고 숨김
    /// </summary>
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;  // 커서 잠금
        Cursor.visible = false;                    // 커서 숨기기
        isCursorLocked = true;
    }

    /// <summary>
    /// 커서 잠금을 해제하고 표시
    /// </summary>
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;  // 커서 잠금 해제
        Cursor.visible = true;                   // 커서 표시
        isCursorLocked = false;
    }
}
