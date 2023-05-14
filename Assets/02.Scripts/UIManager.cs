using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject canvasUI;
    public GameObject inventory;
    public Drop[] itemLists;

    public bool isOpen = false;
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
}