using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemItem : GemStat
{

    public SpriteRenderer spriter;

    public Dictionary<int, GemInfo> dicGem = Managers.Data.dicGemData;
    
    public int gemId = 0;

    public int choice = 0;

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() // SetActive(true)가 되었을 때 실행
    {
        float[] probability = new float[5] { 60f, 25f, 10f, 3.5f, 1.5f };

        choice = Choose(probability);

        //id = id + Random.Range(0, dicGem.Count); // 스폰되는 보석 확률 조정은 여기서 하면 될 듯
        gemId = choice;
        GemInfo gemInfo = dicGem[gemId];
        Id = gemInfo.id;
        Attack = gemInfo.attack;
        Name = gemInfo.name;
        SpriteName = gemInfo.spriteName;
        Sprite sprite = Resources.Load<Sprite>($"Textures/{SpriteName}");

        spriter.sprite = sprite;
    }
    private void OnDisable()// SetActive(false)가 되었을 때 실행
    {
        GetComponentInChildren<Slider>().value = 1.0f;
        Init();
        gemId = 0;
    }

    public int Choose(float[] probs) // float 반환, float 배열 인자로 받음 float 배열이 확률을 나타낸다.
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}
