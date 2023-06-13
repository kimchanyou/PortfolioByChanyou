using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    private RectTransform parentRect;

    void Start()
    {
        parentRect = GetComponent<RectTransform>();
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
            GemInven originGem = transform.GetChild(0).GetComponent<GemInven>();
            GemInven draggingGem = eventData.pointerDrag.transform.GetComponent<GemInven>();
            ToolTip originTool = transform.GetChild(0).GetComponent<ToolTip>();
            if (originGem == null || draggingGem == null)
            {
                Debug.Log("�񱳴�� ����");
                return;
            }
            else
            {
                if (originGem.id != draggingGem.id) // ������ ������ ���� �ٸ��� ��ġ�� �ٲ�
                {
                    originGem.transform.SetParent(Drag.origninTr);
                    originGem.GetComponent<RectTransform>().sizeDelta = Drag.origninTr.GetComponent<RectTransform>().sizeDelta;
                    originGem.transform.localPosition = Vector3.zero;
                    draggingGem.transform.SetParent(this.transform);
                    draggingGem.GetComponent<RectTransform>().sizeDelta = parentRect.sizeDelta;
                }
                else // ������ ������ ���� �ִ뷹���� �ƴϸ� �������鼭 ������ ������ �ö󰡰� ������ ���ŵ�
                {
                    if (originGem.id >= 10)
                    {
                        Debug.Log("�ִ뷹��");
                        return;
                    }
                    GemInfo gemInfo = originGem.dicGem[originGem.id + 1];
                    originGem.id = gemInfo.id;
                    originGem.attack = gemInfo.attack;
                    originGem.gemName = gemInfo.gemName;
                    originGem.spriteName = gemInfo.spriteName;
                    Sprite sprite = Resources.Load<Sprite>($"Textures/{originGem.spriteName}");
                    originGem.gemImage.sprite = sprite;
                    originTool.levelText = "���� : " + (originGem.id + 1).ToString();
                    originTool.attackText = "���ݷ� : " + originGem.attack.ToString();
                    originTool.tipText = originGem.gemName;
                    originTool.itemToShow = originGem.gemImage.sprite;
                    Destroy(draggingGem.gameObject);
                }
            }
        }
    }
}
