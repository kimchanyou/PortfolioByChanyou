using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // 랜덤 좌표에서 보석 스폰
    // 같은 위치에서 스폰되지 않도록
    // 캐릭터가 있는 자리에 스폰되지 않도록
    public static SpawnManager instance;

    public List<GameObject> pool;

    public Vector2 spawnPoints = new Vector2(-9.5f, -9.5f);
    public int spawnVecX = 0;
    public int spawnVecY = 0;

    public int gemLimit = 10;

    WaitForSeconds waitTime = new WaitForSeconds(3f);

    public bool isSpawn = false;

    public void Init()
    {
        pool = new List<GameObject>();
        GemSpawn();
    }

    void Update()
    {
        if(!isSpawn)
            GemSpawn();
    }

    void GemSpawn()
    {
        spawnVecX = Random.Range(0, 26);
        spawnVecY = Random.Range(0, 16);
        Vector3 spawnPoint = new Vector3(spawnPoints.x + spawnVecX, spawnPoints.y + spawnVecY, 0);
        StartCoroutine(GemCreate(spawnPoint));
    }
    IEnumerator GemCreate(Vector3 spawnPoint)
    {
        isSpawn = true;
        pool = PoolManager.pool;
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeSelf)
            {
                pool[i].SetActive(true);
                pool[i].transform.position = spawnPoint;
                break;
            }
        }
        yield return waitTime;
        isSpawn = false;
    }

}
