using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemAttack : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    private BoxCollider2D boxCol2D;

    public Dictionary<int, GemInfo> dicGem;

    public SpriteRenderer spriter;

    private GameObject targetGem = null;
    public GameObject gemItemInven;

    [Header("GemInfo")]
    public int id;
    public float attack;
    public string gemName;
    public string spriteName;

    void Awake()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        boxCol2D = GetComponent<BoxCollider2D>();
        spriter = GetComponent<SpriteRenderer>();
        gemItemInven = Resources.Load<GameObject>("Prefabs/02GemItem"); // 인벤토리에 들어갈 보석 프리팹 로드
    }
    private void OnEnable()
    {
        boxCol2D.enabled = true;
        spriteName = PlayerController.attackSpriteName;
        spriter.sprite = Resources.Load<Sprite>($"Textures/{spriteName}");
        StartCoroutine(GemNotCol());
    }
    private void OnDisable()
    {
        rbody2D.velocity = Vector2.zero;
        targetGem = null;
        //id = 0;
        //attack = 0;
        //gemName = null;
        spriteName = null;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Gem"))
        {
            boxCol2D.enabled = false;
            targetGem = col.gameObject;
            Slider hpbar = targetGem.GetComponentInChildren<Slider>();
            hpbar.value -= 0.2f;
            if (hpbar.value <= 0.001f)
            {
                GameObject clone = Instantiate(gemItemInven, GetItemInven());
                GemInven gemInven = clone.GetComponent<GemInven>();
                GemItem gem = targetGem.GetComponent<GemItem>();
                GemInfo gemInfo = gem.dicGem[gem.gemId];
                gemInven.id = gemInfo.id;
                gemInven.attack = gemInfo.attack;
                gemInven.gemName = gemInfo.gemName;
                gemInven.spriteName = gemInfo.spriteName;
                Managers.Pool.ReturnObject(targetGem, Managers.Pool.itemRoot, Managers.Pool.itemPool);
            }
            StartCoroutine(GemCol());
        }
    }
    private Transform GetItemInven()
    {
        for (int i = 0; i < UIManager.instance.itemInvenLists.Length; i++)
        {
            if (UIManager.instance.itemInvenLists[i].transform.childCount == 0)
            {
                return UIManager.instance.itemInvenLists[i].transform;
            }
        }
        return null;
    }

    IEnumerator GemCol()
    {
        rbody2D.velocity = Vector2.zero;
        rbody2D.AddForce(PlayerController.attackVec.normalized * 100f);

        yield return new WaitForSeconds(0.5f);

        Managers.Pool.ReturnObject(gameObject, Managers.Pool.attackRoot, Managers.Pool.attackPool);
    }
    IEnumerator GemNotCol()
    {
        yield return new WaitForSeconds(2.5f);

        Managers.Pool.ReturnObject(gameObject, Managers.Pool.attackRoot, Managers.Pool.attackPool);
    }
}
