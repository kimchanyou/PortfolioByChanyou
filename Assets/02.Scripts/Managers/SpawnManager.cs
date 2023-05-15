using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // ���� ��ǥ���� ���� ����
    // ���� ��ġ���� �������� �ʵ���
    // ĳ���Ͱ� �ִ� �ڸ��� �������� �ʵ���
    public static SpawnManager instance;

    public List<GameObject> pool;

    public Vector2 spawnPoints = new Vector2(-9.5f, -9.5f);
    public int spawnVecX = 0;
    public int spawnVecY = 0;

    public int gemLimit = 10;

    WaitForSeconds waitTime = new WaitForSeconds(3f);

    public bool isSpawn = false;

    void Start()
    {
        pool = new List<GameObject>();
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
        
        GameObject gem = Managers.Pool.GetObject();
        if (gem != null)
            gem.transform.position = spawnPoint;

        yield return waitTime; // ������ �����Ǵ� �ð�
        isSpawn = false;
    }

}