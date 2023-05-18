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
        // 원래의 인벤토리창을 부모오브젝트로 설정
        origninTr = transform.parent;
        // 드래그가 시작될 때 아이템이 인벤토리 UI에 가려지지 않도록 상위 캔버스의 자식으로 변경함
        this.transform.SetParent(inventoryTr);
        // 드래그가 시작되면 드래그되는 아이템 정보를 저장함
        draggingItem = this.gameObject;
        // 드래그가 시작되면 다른 UI 이벤트를 받지 않도록 설정
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 이벤트가 발생하면 아이템의 위치를 마우스 커서의 위치로 변경
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
        // 드래그가 종료되면 다른 UI 이벤트를 받도록 설정
        canvasGroup.blocksRaycasts = true;
    }
}
