using UnityEngine;
using TMPro;

public class GemID : MonoBehaviour
{
    [HideInInspector]
    public int gemID;

    [SerializeField]
    private TextMeshProUGUI visibleGemID;
    private void Start()
    {
        int gemsVisibleIdNumber = gemID + 1;
        visibleGemID.text = gemsVisibleIdNumber.ToString();
    }

    public void OnTouch()
    {
        GameStateEvents.GemWasTouched(gemID);
    }
}
