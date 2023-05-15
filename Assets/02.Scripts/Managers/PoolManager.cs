using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    public Transform root;
    public GameObject prefabs;

    public Stack<GameObject> pool;

    public void Init()
    {
        if (root == null)
        {
            root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(root);
        }
        prefabs = Resources.Load<GameObject>("Prefabs/01Gems");

        pool = new Stack<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            CreateObject();
        }
    }
    private GameObject CreateObject()
    {
        GameObject newObject = Object.Instantiate(prefabs, root);
        newObject.SetActive(false);
        pool.Push(newObject);

        return newObject;
    }
    
    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject objectInPool = pool.Pop();

            objectInPool.SetActive(true);
            objectInPool.transform.SetParent(null);
            return objectInPool;
        }
        else
        {
            return null;
        }
    }
    public void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(root);
        pool.Push(obj);
    }
}
