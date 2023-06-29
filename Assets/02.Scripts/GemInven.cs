using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ������ �κ��丮�� �� �� ĵ ������ ������ ��Ƽ� ����ǵ��� �ϱ�.
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
        // ��Ÿ���� ���۵Ǹ� �̹����� �ؽ�Ʈ ���
        coolImage.enabled = true;
        coolText.enabled = true;
        float coolTime = 3f;
        isAttack = true;
        while (coolTime > 0)
        {
            // 3�� ���� ��Ÿ�� ���� �ǵ���
            coolTime -= Time.deltaTime;
            coolImage.fillAmount = coolTime / 3f;
            // �Ҽ��� ���ڸ������� ǥ�� �ǵ���
            coolText.text = coolTime.ToString("0.0");
            yield return new WaitForFixedUpdate();
        }
        // ��Ÿ���� ������ �̹����� �ؽ�Ʈ �Ⱥ��̵���
        coolImage.enabled = false;
        coolText.enabled = false;
        isCool = false;
        isAttack = false;
    }
}
