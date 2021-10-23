using System.Collections;
using UnityEngine;
using TMPro;

public class GemController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI visibleGemID;
    [SerializeField] private Sprite defaultLook;
    [SerializeField] private Sprite selectedLook;

    [HideInInspector] public int gemID;

    private SpriteRenderer gemRenderer;

    private void Start()
    {
        int gemsVisibleIdNumber = gemID + 1;
        visibleGemID.text = gemsVisibleIdNumber.ToString();

        gemRenderer = GetComponentInChildren<SpriteRenderer>();
        gemRenderer.sprite = defaultLook;
    }

    public void OnTouch()
    {
        GameEvents.GemWasTouched(gemID);
    }

    public void ChangeSprite()
    {
        gemRenderer.sprite = selectedLook;
    }

    public void TextDissapear()
    {
        StartCoroutine(FadeOutSlowly());
    }

    IEnumerator FadeOutSlowly()
    {
        float alphaValue = visibleGemID.color.a;

        while (alphaValue > 0)
        {
            alphaValue -= Time.deltaTime;
            visibleGemID.color = new Color(visibleGemID.color.r, visibleGemID.color.g, visibleGemID.color.b, alphaValue);
            yield return null;
        }
        Destroy(visibleGemID);
    }
}
