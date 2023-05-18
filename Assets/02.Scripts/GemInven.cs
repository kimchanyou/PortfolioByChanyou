using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 보석이 인벤토리에 들어갈 때 캔 보석의 정보를 담아서 적용되도록 하기.
public class GemInven : MonoBehaviour
{
    public Dictionary<int, GemInfo> dicGem;

    public Image gemImage;
    public TextMeshProUGUI levelText;

    [Header("GemInfo")]
    public int id;
    public float attack;
    public string gemName;
    public string spriteName;

    void Start()
    {
        dicGem = Managers.Data.dicGemData;
        gemImage = GetComponent<Image>();
        gemImage.sprite = Resources.Load<Sprite>($"Textures/{spriteName}");
        levelText = GetComponentInChildren<TextMeshProUGUI>();
        levelText.text = "LV " + (id + 1);
    }

    void Update()
    {
        
    }
}
