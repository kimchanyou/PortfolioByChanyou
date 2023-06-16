using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager instance;

    public Image itemImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI tipText;

    public RectTransform tipWindow;

    public static Action<string, string, string, Sprite, Vector2> OnMouseTip;
    public static Action OnMouseLoseFocus;

    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;

        //    DontDestroyOnLoad(this.gameObject);
        //}
        //else
        //{
        //    Destroy(this.gameObject);
        //}
    }
    private void Start()
    {
        tipWindow = transform.GetChild(1).GetComponent<RectTransform>();
        itemImage = tipWindow.GetChild(0).GetComponent<Image>();
        levelText = tipWindow.GetChild(1).GetComponent<TextMeshProUGUI>();
        attackText = tipWindow.GetChild(2).GetComponent<TextMeshProUGUI>();
        tipText = tipWindow.GetChild(3).GetComponent<TextMeshProUGUI>();
        HideTip();
    }
    private void OnEnable()
    {
        OnMouseTip += ShowTip;
        OnMouseLoseFocus += HideTip;
    }
    private void OnDisable()
    {
        OnMouseTip -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }
    private void ShowTip(string level, string attack, string tip, Sprite item, Vector2 pos)
    {
        levelText.text = level;
        //attackText.text = attack;
        tipText.text = tip;
        itemImage.sprite = item;
        tipWindow.sizeDelta = new Vector2(350, 250);

        tipWindow.gameObject.SetActive(true);
        tipWindow.transform.position = new Vector2(pos.x - 20, pos.y + 20);
    }
    private void HideTip()
    {
        levelText.text = default;
        //attackText.text = default;
        tipText.text = default;
        itemImage.sprite = default;
        tipWindow.gameObject.SetActive(false);
    }
}
