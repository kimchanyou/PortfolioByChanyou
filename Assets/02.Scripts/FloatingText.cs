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
            //vector.Set(text.transform.position.x, text.transform.position.y + (moveSpeed * Time.deltaTime));
            vector.Set(rectTr.anchoredPosition.x, rectTr.anchoredPosition.y + (moveSpeed * Time.deltaTime));
            rectTr.anchoredPosition = vector;

            destroyTime -= Time.deltaTime;

            if (destroyTime <= 0)
            {
                destroyTime = 1.5f;
                gameObject.SetActive(false);
                rectTr.anchoredPosition = startPosition;
            }
        }
    }

    public void ShowText(string showText)
    {
        gameObject.SetActive(true);
        rectTr.anchoredPosition = startPosition;

        destroyTime = 1.5f;
        text.text = showText;
    }
}