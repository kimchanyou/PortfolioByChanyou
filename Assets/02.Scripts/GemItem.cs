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

    private void OnEnable() // SetActive(true)�� �Ǿ��� �� ����
    {
        float[] probability = new float[5] { 60f, 25f, 10f, 3.5f, 1.5f };

        choice = Choose(probability);

        //id = id + Random.Range(0, dicGem.Count); // �����Ǵ� ���� Ȯ�� ������ ���⼭ �ϸ� �� ��
        gemId = choice;
        GemInfo gemInfo = dicGem[gemId];
        Id = gemInfo.id;
        Attack = gemInfo.attack;
        Name = gemInfo.name;
        SpriteName = gemInfo.spriteName;
        Sprite sprite = Resources.Load<Sprite>($"Textures/{SpriteName}");

        spriter.sprite = sprite;
    }
    private void OnDisable()// SetActive(false)�� �Ǿ��� �� ����
    {
        GetComponentInChildren<Slider>().value = 1.0f;
        Init();
        gemId = 0;
    }

    public int Choose(float[] probs) // float ��ȯ, float �迭 ���ڷ� ���� float �迭�� Ȯ���� ��Ÿ����.
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
