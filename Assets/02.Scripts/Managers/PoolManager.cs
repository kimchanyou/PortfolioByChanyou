using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    public Transform itemRoot;
    public Transform attackRoot;

    public GameObject gemItemPrefabs;
    public GameObject gemAttackPrefabs;

    public Stack<GameObject> itemPool;
    public Stack<GameObject> attackPool;

    public void Init()
    {
        if (itemRoot == null)
        {
            itemRoot = new GameObject { name = "@Item_Root" }.transform;
            Object.DontDestroyOnLoad(itemRoot);
        }
        if (attackRoot == null)
        {
            attackRoot = new GameObject { name = "@Attack_Root" }.transform;
            Object.DontDestroyOnLoad(attackRoot);
        }
        gemItemPrefabs = Resources.Load<GameObject>("Prefabs/01Gems");
        gemAttackPrefabs = Resources.Load<GameObject>("Prefabs/03GemAttack");

        itemPool = new Stack<GameObject>();
        attackPool = new Stack<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            CreateObject(gemItemPrefabs, itemRoot, itemPool);
        }
        for (int i = 0; i < 9; i++)
        {
            CreateObject(gemAttackPrefabs, attackRoot, attackPool);
        }
    }
    private GameObject CreateObject(GameObject prefab, Transform parent, Stack<GameObject> pool)
    {
        GameObject newObject = Object.Instantiate(prefab, parent);
        newObject.SetActive(false);
        pool.Push(newObject);

        return newObject;
    }
    
    public GameObject GetObject(Stack<GameObject> pool)
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
    public void ReturnObject(GameObject obj, Transform parent, Stack<GameObject> pool)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parent);
        pool.Push(obj);
    }
}
