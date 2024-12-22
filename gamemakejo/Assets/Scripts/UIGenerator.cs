using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGenerator : MonoBehaviour
{
    public Canvas canvas;
    public GameObject textPrefab;
    private TextMeshProUGUI timeText;
    private float elapsedTime = 0f;

    private GameObject specialMessage;
    public GameObject specialMessagePrefab;

    private bool isSpecialMessageActivated = false; // 특별 메시지 활성화 상태 체크

    void Start()
    {
        CreateText(new Vector2(0, -50), "00:00");

        // 특별 메시지 UI 초기화
        if (specialMessagePrefab != null && canvas != null)
        {
            specialMessage = Instantiate(specialMessagePrefab, canvas.transform);
            specialMessage.SetActive(false); // 초기에는 비활성화
        }
        else
        {
            Debug.LogWarning("SpecialMessagePrefab or Canvas is not assigned.");
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        float seconds = elapsedTime % 60;
        timeText.text = string.Format("Time: {0:D2}:{1:D2}", minutes, Mathf.FloorToInt(seconds));

        // 150초 후에 특별 메시지 활성화
        if (!isSpecialMessageActivated && elapsedTime >= 60f)
        {
            ActivateSpecialMessage();
        }
    }

    void CreateText(Vector2 position, string initialText)
    {
        GameObject textObj = Instantiate(textPrefab, canvas.transform);

        // RectTransform을 가져와서 위치를 화면 상단 중앙에 설정
        RectTransform rectTransform = textObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;  // (0, -50)으로 시작 위치 설정

        // Anchor 설정 (상단 중앙)
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);

        // 텍스트 내용 설정
        timeText = textObj.GetComponent<TextMeshProUGUI>();
        timeText.text = initialText;
    }

    void ActivateSpecialMessage()
    {
        if (specialMessage != null)
        {
            specialMessage.SetActive(true); // 특별 메시지 활성화
            Debug.Log("Special message activated!");
        }
        isSpecialMessageActivated = true; // 상태 갱신
    }
}