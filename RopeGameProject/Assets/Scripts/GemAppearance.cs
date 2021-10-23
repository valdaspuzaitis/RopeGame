using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemAppearance : MonoBehaviour
{
    [SerializeField]
    private Sprite defaultLook;
    [SerializeField]
    private Sprite selectedLook;

    private Image appearance;

    private SpriteRenderer gemRenderer;

    void Awake()
    {
        gemRenderer = GetComponentInChildren<SpriteRenderer>();
        gemRenderer.sprite = defaultLook;
    }

    public void ChangeSprite()
    {
        gemRenderer.sprite = selectedLook;
    }
}
