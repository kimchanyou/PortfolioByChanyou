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
    protected string name;
    [SerializeField]
    protected string spriteName;

    public int Id { get { return id; } set { id = value; } }
    public float Attack { get { return attack; } set { attack = value; } }
    public string Name { get { return name; } set { name = value; } }
    public string SpriteName { get { return spriteName; } set { spriteName = value; } }

    void Start()
    {
        id = 0;
        attack = 0;
        name = null;
        spriteName = null;
    }

}
