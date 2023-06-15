using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string levelText;
    public string attackText;
    public string tipText;
    public Sprite itemToShow;
    private float timeToWait = 0.2f;

    void Start()
    {
        levelText = "���� : " + (GetComponent<GemInven>().id + 1).ToString();
        attackText = "���ݷ� : " + GetComponent<GemInven>().attack.ToString();
        tipText = GetComponent<GemInven>().gemName;
        itemToShow = GetComponent<GemInven>().gemImage.sprite;
    }

    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        ToolTipManager.OnMouseLoseFocus();
    }
    private void ShowMessage()
    {
        ToolTipManager.OnMouseTip(levelText, attackText, tipText, itemToShow, transform.position);
    }
    private IEnumerator StartTimer()
    {
        // 0.5�� �ִٰ� �޽��� �����ش�.
        yield return new WaitForSeconds(timeToWait);

        ShowMessage();
    }
}
