using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemAttack : MonoBehaviour
{
    private float attackSpeed = 100f;

    private Rigidbody2D rbody2D;

    public Dictionary<int, GemInfo> dicGem;

    public SpriteRenderer spriter;

    [Header("GemInfo")]
    public int id;
    public float attack;
    public string gemName;
    public string spriteName;

    void Awake()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        //dicGem = Managers.Data.dicGemData;

        //rbody2D.AddForce(transform.right * attackSpeed);

    }
    private void OnDisable()
    {
        
    }
}
