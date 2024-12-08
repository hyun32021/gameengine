using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private float expandDuration = 0.5f; // 폭발 확장 시간
    [SerializeField] private float holdDuration = 0.5f; // 유지 시간

    private float currentTime = 0f;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private bool expanding = true;

    // 공격 범위에 따른 크기 설정
    public void SetExplosionSize(float attackRange)
    {
        targetScale = Vector3.one * attackRange;
    }

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (expanding)
        {
            // 확장 중
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / expandDuration);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            if (currentTime >= expandDuration)
            {
                expanding = false; // 확장 완료
                currentTime = 0f; // 시간 초기화
            }
        }
        else
        {
            // 유지 후 제거
            currentTime += Time.deltaTime;
            if (currentTime >= holdDuration)
            {
                Destroy(gameObject);
            }
        }
    }
}
