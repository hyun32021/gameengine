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

    public GameObject crosshairPrefab; // ������ UI �������� ����
    private GameObject crosshair;

    private GameObject deadMasage;
    public GameObject deadPrefab;

    void Start()
    {
        CreateText(new Vector2(0, -50), "��� �ð�: 00:00");

        // ȭ�� �߾ӿ� �������� ����
        crosshair = Instantiate(crosshairPrefab, canvas.transform);
        crosshair.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20, -10);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        float seconds = elapsedTime % 60;
        timeText.text = string.Format("{0:D2}:{1:D2}", minutes, Mathf.FloorToInt(seconds));
    }

    void CreateText(Vector2 position, string initialText)
    {
        GameObject textObj = Instantiate(textPrefab, canvas.transform);

        // RectTransform�� �����ͼ� ��ġ�� ȭ�� ��� �߾ӿ� ����
        RectTransform rectTransform = textObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;  // (0, 100)���� ���� ��ġ ����

        // Anchor ���� (��� �߾�)
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);

        // �ؽ�Ʈ ���� ����
        timeText = textObj.GetComponent<TextMeshProUGUI>();
        timeText.text = initialText;
    }
}

