using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeOrbit : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1200f;
    
    [SerializeField]private float duration = 0.35f; // 회전 지속 시간

    private float timer = 0f;

    void Update()
    {
        /// 도끼 회전
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // 경과 시간 증가
        timer += Time.deltaTime;

        // 특정 시점에 소리 재생
        if (timer >= 0.1f && timer < 0.2f)
        {
            // 휘두르는 소리 재생 (한 번만 실행되도록 체크 필요)
            Debug.Log("휘두르는 소리 재생!");
        }

        // 시간이 지나면 제거
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
