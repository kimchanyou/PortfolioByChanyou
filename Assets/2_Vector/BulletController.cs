using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D bulletRigidbody2D;
    private float bulletSpeed = 10f;
    void Start()
    {
        bulletRigidbody2D = GetComponent<Rigidbody2D>();
        bulletRigidbody2D.velocity = bulletSpeed * transform.right;
    }
}
