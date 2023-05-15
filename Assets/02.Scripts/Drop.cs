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
            //
        }
    }
}
