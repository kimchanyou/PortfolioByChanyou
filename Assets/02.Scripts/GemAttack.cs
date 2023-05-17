using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemAttack : MonoBehaviour
{
    public Dictionary<int, GemInfo> dicGem;

    public SpriteRenderer spriter;

    [Header("GemInfo")]
    public int id;
    public float attack;
    public string gemName;
    public string spriteName;

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        dicGem = Managers.Data.dicGemData;


    }
    private void OnDisable()
    {
        
    }
}
