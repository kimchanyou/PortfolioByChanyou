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

    public Image coolImage;
    public TextMeshProUGUI coolText;

    [Header("GemInfo")]
    public int id;
    public float attack;
    public string gemName;
    public string spriteName;

    public bool isCool = false;
    private bool isAttack = false;

    void Start()
    {
        dicGem = Managers.Data.dicGemData;
        gemImage = GetComponent<Image>();
        gemImage.sprite = Resources.Load<Sprite>($"Textures/{spriteName}");
        coolImage.enabled = false;
        coolText.enabled = false;
    }

    void Update()
    {
        if (isCool && !isAttack)
        {
            StartCoroutine(IsAttackTrue());
        }
    }

    IEnumerator IsAttackTrue()
    {
        coolImage.enabled = true;
        coolText.enabled = true;
        float coolTime = 3f;
        isAttack = true;
        while (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
            coolImage.fillAmount = coolTime / 3f;
            coolText.text = coolTime.ToString("0.0");
            yield return new WaitForFixedUpdate();
        }
        coolImage.enabled = false;
        coolText.enabled = false;
        isCool = false;
        isAttack = false;
    }
}
