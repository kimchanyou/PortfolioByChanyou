using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject canvasUI;
    public GameObject inventory;
    public Drop[] itemInvenLists;
    public GemInven[] itemLists;
    public GameObject temp;

    public bool isOpen = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        canvasUI = this.gameObject;
        inventory = canvasUI.transform.GetChild(0).gameObject;
        itemInvenLists = GetComponentsInChildren<Drop>();
        inventory.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isOpen)
            {
                inventory.SetActive(true);
                isOpen = true;
            }
            else
            {
                inventory.SetActive(false);
                isOpen = false;
            }
        }
    }
    public void SortAll()
    {
        TrimAll();
        // 전체 보석을 검사해서 비교 후 sort정렬하기
        itemLists = GetComponentsInChildren<GemInven>();
        if (itemLists == null) return;
        //for (int i = 0; i < itemLists.Length - 1; i++)
        //{
        //    for (int j = 0; j < itemLists.Length - i - 1; j++)
        //    {
        //        if (itemLists[j].id < itemLists[(j + 1)].id)
        //        {
        //            temp.transform.SetParent(itemLists[j].transform.parent);
        //            itemLists[j].transform.SetParent(itemLists[(j + 1)].transform.parent);
        //            itemLists[(j + 1)].transform.SetParent(temp.transform.parent);
        //            itemLists[j].transform.localPosition = Vector3.zero;
        //            itemLists[(j + 1)].transform.localPosition = Vector3.zero;
        //            temp.transform.parent = null;
        //            GemInven gemTemp = itemLists[j];
        //            itemLists[j] = itemLists[(j + 1)];
        //            itemLists[(j + 1)] = gemTemp;
        //        }
        //    }
        //}
        for (int i = 0; i < itemLists.Length - 1; i++)
        {
            for (int j = i + 1; j < itemLists.Length; j++)
            {
                if (itemLists[i].id < itemLists[j].id)
                {
                    temp.transform.SetParent(itemLists[i].transform.parent, false);
                    itemLists[i].transform.SetParent(itemLists[j].transform.parent, false);
                    itemLists[j].transform.SetParent(temp.transform.parent, false);
                    itemLists[i].transform.localPosition = Vector3.zero;
                    itemLists[j].transform.localPosition = Vector3.zero;
                    GemInven gemTemp = itemLists[i];
                    itemLists[i] = itemLists[j];
                    itemLists[j] = gemTemp;
                    temp.transform.parent = null;
                }
            }
        }
    }

    public void TrimAll()
    {
        for (int i = 0; i < itemInvenLists.Length; i++)
        {
            if (itemInvenLists[i].transform.childCount == 0)
            {
                for (int j = i; j < itemInvenLists.Length; j++)
                {
                    if (itemInvenLists[j].transform.childCount != 0)
                    {
                        GameObject trimGem = itemInvenLists[j].transform.GetChild(0).gameObject;
                        trimGem.transform.SetParent(itemInvenLists[i].transform);
                        trimGem.transform.localPosition = Vector3.zero;
                        break;
                    }
                }
            }
        }
    }
}