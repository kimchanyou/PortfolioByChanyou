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
        // 쿨타임이 시작되면 이미지와 텍스트 출력
        coolImage.enabled = true;
        coolText.enabled = true;
        float coolTime = 3f;
        isAttack = true;
        while (coolTime > 0)
        {
            // 3초 동안 쿨타임 진행 되도록
            coolTime -= Time.deltaTime;
            coolImage.fillAmount = coolTime / 3f;
            // 소수점 한자리까지만 표시 되도록
            coolText.text = coolTime.ToString("0.0");
            yield return new WaitForFixedUpdate();
        }
        // 쿨타임이 끝나면 이미지와 텍스트 안보이도록
        coolImage.enabled = false;
        coolText.enabled = false;
        isCool = false;
        isAttack = false;
    }
}
