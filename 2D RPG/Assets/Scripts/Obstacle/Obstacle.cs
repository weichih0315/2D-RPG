using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Color defaultColor, fadedColor;

    void Start()
    {
        defaultColor = spriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.5f;
    }

    public void FadeIn()
    {
        spriteRenderer.color = defaultColor;
    }

    public void FadeOut()
    {
        spriteRenderer.color = fadedColor;
    }
}
