using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    public GameObject go;
    public GameObject prefabs;

    public List<GameObject> pool;

    public void Init()
    {
        go = GameObject.Find("@Managers");
        prefabs = Resources.Load<GameObject>("Prefabs/01Gems");

        pool = new List<GameObject>();
    }
    private void Update()
    {
        Get();
    }

    public GameObject Get(int poolCount = 10)
    {
        GameObject select = null;

        if (pool.Count < poolCount)
        {
            select = Object.Instantiate(prefabs, go.transform);
            select.SetActive(false);
            pool.Add(select);
        }

        return select;
    }
}
