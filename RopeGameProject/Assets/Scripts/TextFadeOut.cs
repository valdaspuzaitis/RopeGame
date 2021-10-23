using System.Collections;
using UnityEngine;
using TMPro;

public class TextFadeOut : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;

    public void TextDissapear()
    {
        StartCoroutine(FadeOutSlowly());
    }

    IEnumerator FadeOutSlowly()
    {
        float alphaValue = numberText.color.a;

        while (alphaValue > 0)
        {
            alphaValue -= Time.deltaTime;
            numberText.color = new Color(numberText.color.r, numberText.color.g, numberText.color.b, alphaValue);
            yield return null;
        }
        Destroy(numberText);
    }
}
