using UnityEngine;
using TMPro;

public class GemID : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI visibleGemID;

    [HideInInspector] public int gemID;

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
