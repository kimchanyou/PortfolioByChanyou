using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    public static Attackable instance;

    public bool isAttack;

    void Start()
    {
        isAttack = true;
    }

    void Update()
    {
        
    }
}
