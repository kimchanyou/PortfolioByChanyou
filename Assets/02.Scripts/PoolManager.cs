using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance; // ΩÃ±€≈Ê

    public GameObject prefabs;

    public List<GameObject> pool;

    private void Awake()
    {
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

        //foreach (GameObject item in pool)
        //{
        //    if (!item.activeSelf)
        //    {
        //        select = item;
        //        select.SetActive(false);
        //        break;
        //    }
        //}

        //if (select == null)
        //{
            if (pool.Count < poolCount)
            {
                select = Instantiate(prefabs, transform);
                select.SetActive(false);
                pool.Add(select);
            }
        //}

        return select;
    }
}
