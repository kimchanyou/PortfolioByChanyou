using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject prefabs;
    public GameObject go;

    public static List<GameObject> pool;

    public void Init()
    {
        prefabs = Resources.Load<GameObject>("Prefabs/01Gems");
        go = GameObject.Find("@Managers");

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
            select = Instantiate(prefabs, go.transform);
            select.SetActive(false);
            pool.Add(select);
        }

        return select;
    }
}
