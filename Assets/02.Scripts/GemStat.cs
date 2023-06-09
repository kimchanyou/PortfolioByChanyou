using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemStat : MonoBehaviour
{
    [SerializeField]
    protected int id;
    [SerializeField]
    protected float attack;
    [SerializeField]
    protected string gemName;
    [SerializeField]
    protected string spriteName;

    public int Id { get { return id; } set { id = value; } }
    public float Attack { get { return attack; } set { attack = value; } }
    public string GemName { get { return gemName; } set { gemName = value; } }
    public string SpriteName { get { return spriteName; } set { spriteName = value; } }

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        id = 0;
        attack = 0;
        gemName = null;
        spriteName = null;
    }
}
