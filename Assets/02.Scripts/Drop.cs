using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    private RectTransform parentRect;
    private UIManager canvas_UI;

    void Start()
    {
        parentRect = GetComponent<RectTransform>();
        canvas_UI = FindObjectOfType<UIManager>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        //GameObject dropped = eventData.pointerDrag;
        //Drag dragItem = dropped.GetComponent<Drag>();

        if (transform.childCount == 0)
        {
            Drag.draggingItem.transform.SetParent(this.transform);
            Drag.draggingItem.GetComponent<RectTransform>().sizeDelta = parentRect.sizeDelta;
            //dragItem.transform.SetParent(this.transform);
            //dropped.transform.SetParent(this.transform);
        }
        else
        {
            FloatingText floatingText = canvas_UI.transform.GetChild(4).GetComponent<FloatingText>();
            GemInven originGem = transform.GetChild(0).GetComponent<GemInven>();
            GemInven draggingGem = eventData.pointerDrag.transform.GetComponent<GemInven>();
            ToolTip originTool = transform.GetChild(0).GetComponent<ToolTip>();
            if (originGem == null || draggingGem == null)
            {
                Debug.Log("비교대상 없음");
                return;
            }
            else
            {
                if (originGem.id != draggingGem.id) // 보석의 레벨이 서로 다르면 위치만 바뀜
                {
                    originGem.transform.SetParent(Drag.origninTr);
                    originGem.GetComponent<RectTransform>().sizeDelta = Drag.origninTr.GetComponent<RectTransform>().sizeDelta;
                    originGem.transform.localPosition = Vector3.zero;
                    draggingGem.transform.SetParent(this.transform);
                    draggingGem.GetComponent<RectTransform>().sizeDelta = parentRect.sizeDelta;
                }
                else // 보석의 레벨이 같고 최대레벨이 아니면 합쳐지면서 보석의 레벨이 올라가고 정보가 갱신됨
                {
                    if (originGem.id >= 10)
                    {
                        floatingText.ShowText("최대 레벨 입니다.");
                        Debug.Log("최대레벨");
                        return;
                    }
                    GemInfo gemInfo = originGem.dicGem[originGem.id + 1];
                    originGem.id = gemInfo.id;
                    originGem.attack = gemInfo.attack;
                    originGem.gemName = gemInfo.gemName;
                    originGem.spriteName = gemInfo.spriteName;
                    Sprite sprite = Resources.Load<Sprite>($"Textures/{originGem.spriteName}");
                    originGem.gemImage.sprite = sprite;
                    originTool.levelText = "레벨 : " + (originGem.id + 1).ToString();
                    originTool.attackText = "공격력 : " + originGem.attack.ToString();
                    originTool.tipText = originGem.gemName;
                    originTool.itemToShow = originGem.gemImage.sprite;
                    floatingText.ShowText("레벨 " + (originGem.id + 1).ToString() + "보석 합성 성공");
                    Destroy(draggingGem.gameObject);
                }
            }
        }
    }
}
