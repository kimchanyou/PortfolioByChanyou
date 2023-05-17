using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    void Start()
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        //GameObject dropped = eventData.pointerDrag;
        //Drag dragItem = dropped.GetComponent<Drag>();

        if (transform.childCount == 0)
        {
            Drag.draggingItem.transform.SetParent(this.transform);
            //dragItem.transform.SetParent(this.transform);
            //dropped.transform.SetParent(this.transform);
        }
        else
        {
            GemInven originGem = transform.GetChild(0).GetComponent<GemInven>();
            GemInven draggingGem = eventData.pointerDrag.transform.GetComponent<GemInven>();
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
                    originGem.transform.localPosition = Vector3.zero;
                    draggingGem.transform.SetParent(this.transform);
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
                    originGem.levelText.text = "LV " + (originGem.id + 1);
                    Destroy(draggingGem.gameObject);
                }
            }
        }
    }
}
