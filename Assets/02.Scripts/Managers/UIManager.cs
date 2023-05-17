using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject canvasUI;
    public GameObject inventory;
    public Drop[] itemLists;

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
        itemLists = GetComponentsInChildren<Drop>();
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

    public void TrimAll()
    {
        //int i = 0;
        //while (itemLists[i++].transform.childCount != 0) ;
        //int j = i;

        //while (true)
        //{
        //    while (++j < itemLists.Length && itemLists[j] == null) ;

        //    if (j == itemLists.Length) break;

        //    GameObject trimGem = itemLists[j].transform.GetChild(0).gameObject;
        //    trimGem.transform.SetParent(itemLists[i].transform);
        //    trimGem.transform.localPosition = Vector3.zero;

        //}

        for (int i = 0; i < itemLists.Length; i++)
        {
            if (itemLists[i].transform.childCount == 0)
            {
                for (int j = i; j < itemLists.Length; j++)
                {
                    if (itemLists[j].transform.childCount != 0)
                    {
                        GameObject trimGem = itemLists[j].transform.GetChild(0).gameObject;
                        trimGem.transform.SetParent(itemLists[i].transform);
                        trimGem.transform.localPosition = Vector3.zero;
                        break;
                    }
                }
            }
        }
    }
}