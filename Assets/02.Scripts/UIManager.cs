using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject canvasUI;
    public GameObject inventory;

    public bool isOpen = false;
    void Start()
    {
        canvasUI = GameObject.Find("Canvas_UI");
        inventory = canvasUI.transform.GetChild(0).gameObject;
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