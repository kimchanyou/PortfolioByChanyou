using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    public static GameObject targetGem = null;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Gem"))
        {
            targetGem = col.gameObject; 
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Gem"))
        {
            targetGem = null;
        }
    }
}
