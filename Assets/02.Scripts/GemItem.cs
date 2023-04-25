using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemItem : GemStat
{
    public Sprite[] gemImages;

    public SpriteRenderer spriter;

    public Dictionary<int, GemInfo> dicGem = Managers.Data.dicGemData;
    
    int id = 101;

    private void Awake()
    {
        gemImages = Resources.LoadAll<Sprite>("Textures/");
        spriter = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() // SetActive(true)가 되었을 때 실행
    {
        id = id + Random.Range(0, dicGem.Count);
        GemInfo gemInfo = dicGem[id];
        Id = gemInfo.id;
        Attack = gemInfo.attack;
        Name = gemInfo.name;
        SpriteName = gemInfo.spriteName;

        spriter.sprite = Resources.Load<Sprite>($"Textures/{SpriteName}");
        //spriter.sprite = gemImages[Random.Range(0, gemImages.Length)];
        
    }
    private void OnDisable()// SetActive(false)가 되었을 때 실행
    {
        GetComponentInChildren<Slider>().value = 1.0f;
    }
}
