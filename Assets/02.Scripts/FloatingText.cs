using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 40f;
    public float destroyTime = 1.5f;

    public TextMeshProUGUI text;
    private RectTransform rectTr;

    private Vector2 vector;
    private Vector2 startPosition = new Vector2(0, 150f);

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        rectTr = GetComponent<RectTransform>();
        rectTr.anchoredPosition = startPosition;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            // floatingText의 포지션이 위로 일정 속도로 상승
            vector.Set(rectTr.anchoredPosition.x, rectTr.anchoredPosition.y + (moveSpeed * Time.deltaTime));
            rectTr.anchoredPosition = vector;

            destroyTime -= Time.deltaTime;

            if (destroyTime <= 0) // 시간이 지나면 floatingText가 비활성화 되고 위치 초기화
            {
                destroyTime = 1.5f;
                gameObject.SetActive(false);
                rectTr.anchoredPosition = startPosition;
            }
        }
    }

    // 보석 획득 및 합성시 해당 문구가 뜰 수 있도록 호출해서 사용
    public void ShowText(string showText)
    {
        gameObject.SetActive(true);
        rectTr.anchoredPosition = startPosition;

        destroyTime = 1.5f;
        text.text = showText;
    }
}