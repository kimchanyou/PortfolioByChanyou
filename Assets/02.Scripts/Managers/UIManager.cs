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
    public Drop[] itemWeaponLists;
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
    public void SortAll() // Trim을 한 후 아이템 레벨을 비교해서 내림차순으로 정렬
    {
        TrimAll();
        // 전체 보석을 검사해서 비교 후 sort정렬하기
        itemLists = GetComponentsInChildren<GemInven>();
        if (itemLists == null) return;
        
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
                    temp.transform.SetParent(null);
                }
            }
        }
    }

    public void TrimAll() // 인벤토리의 빈공간을 검사 후 아이템을 빈 공간 없이 정렬
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
                        trimGem.transform.SetParent(itemInvenLists[i].transform, false);
                        trimGem.transform.localPosition = Vector3.zero;
                        break;
                    }
                }
            }
        }
    }
    public void SortWeapon()
    {
        for (int i = 0; i < itemWeaponLists.Length; i++)
        {
            if (itemWeaponLists[i].transform.childCount == 0)
            {
                for (int j = i; j < itemWeaponLists.Length; j++)
                {
                    if (itemWeaponLists[j].transform.childCount != 0)
                    {
                        GameObject trimGem = itemWeaponLists[j].transform.GetChild(0).gameObject;
                        trimGem.transform.SetParent(itemWeaponLists[i].transform, false);
                        trimGem.transform.localPosition = Vector3.zero;
                        break;
                    }
                }
            }
        }

        itemLists = GameObject.Find("Attack").GetComponentsInChildren<GemInven>();
        if (itemLists == null) return;

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
                    temp.transform.SetParent(null);
                }
            }
        }
    }
}