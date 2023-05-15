using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������ �κ��丮�� �� �� ĵ ������ ������ ��Ƽ� ����ǵ��� �ϱ�.
public class GemInven : MonoBehaviour
{
    public Image gemImage;

    [Header("GemInfo")]
    public int id;
    public float attack;
    public string gemName;
    public string spriteName;

    void Start()
    {
        gemImage = GetComponent<Image>();
        gemImage.sprite = Resources.Load<Sprite>($"Textures/{spriteName}");
    }

    void Update()
    {
        
    }
}
