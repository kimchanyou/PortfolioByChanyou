using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemItem : MonoBehaviour
{
    public Sprite[] gemImages;

    public SpriteRenderer spriter;

    private void Awake()
    {
        gemImages = Resources.LoadAll<Sprite>("Textures/");
        spriter = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() // SetActive(true)가 되었을 때 실행
    {
        spriter.sprite = gemImages[Random.Range(0, gemImages.Length)];
    }
    private void OnDisable()// SetActive(false)가 되었을 때 실행
    {
        GetComponentInChildren<Slider>().value = 1.0f;
    }
}
