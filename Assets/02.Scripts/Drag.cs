using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform gemItemTr;
    public static Transform origninTr;
    private Transform inventoryTr;
    private CanvasGroup canvasGroup;

    public static GameObject draggingItem = null;

    void Start()
    {
        gemItemTr = GetComponent<Transform>();
        inventoryTr = GameObject.FindGameObjectWithTag("CanvasUI").transform;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ������ �κ��丮â�� �θ������Ʈ�� ����
        origninTr = transform.parent;
        // �巡�װ� ���۵� �� �������� �κ��丮 UI�� �������� �ʵ��� ���� ĵ������ �ڽ����� ������
        this.transform.SetParent(inventoryTr);
        // �巡�װ� ���۵Ǹ� �巡�׵Ǵ� ������ ������ ������
        draggingItem = this.gameObject;
        // �巡�װ� ���۵Ǹ� �ٸ� UI �̺�Ʈ�� ���� �ʵ��� ����
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �巡�� �̺�Ʈ�� �߻��ϸ� �������� ��ġ�� ���콺 Ŀ���� ��ġ�� ����
        gemItemTr.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gemItemTr.parent == inventoryTr)
        {
            gemItemTr.SetParent(origninTr);
        }
        transform.localPosition = Vector3.zero;

        draggingItem = null;
        origninTr = null;
        // �巡�װ� ����Ǹ� �ٸ� UI �̺�Ʈ�� �޵��� ����
        canvasGroup.blocksRaycasts = true;
    }
}
