using UnityEngine;

public class GemAppearance : MonoBehaviour
{
    [SerializeField] private Sprite defaultLook;
    [SerializeField] private Sprite selectedLook;

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
