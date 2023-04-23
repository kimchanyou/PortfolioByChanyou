using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // 랜덤 좌표에서 보석 스폰
    // 같은 위치에서 스폰되지 않도록
    // 캐릭터가 있는 자리에 스폰되지 않도록
    public static SpawnManager instance;

    public Vector2 spawnPoints = new Vector2(-12.8f, -7.28f);
    public int spawnVecX = 0;
    public int spawnVecY = 0;

    public int gemCount = 0;

    public int gemLimit = 10;

    WaitForSeconds waitTime = new WaitForSeconds(3f);

    GameObject[] gems;

    public bool isSpawn = true;
    void Start()
    {
        gems = Resources.LoadAll<GameObject>("Prefabs/");
    }

    void Update()
    {
        if (isSpawn)
            GemSpawn();
    }

    void GemSpawn()
    {
        if (gemCount < gemLimit)
        {
            spawnVecX = Random.Range(0, 26);
            spawnVecY = Random.Range(0, 16);
            Vector3 spawnPoint = new Vector3(spawnPoints.x + spawnVecX, spawnPoints.y + spawnVecY, 0);
            StartCoroutine(GemCreate(spawnPoint));
        }
    }
    IEnumerator GemCreate(Vector3 spawnPoint)
    {
        isSpawn = false;
        GameObject go = gems[Random.Range(0, gems.Length)];
        GameObject clone = Instantiate(go, spawnPoint, Quaternion.identity);
        gemCount++;
        yield return waitTime;
        isSpawn = true;
    }

}
