using UnityEngine;

public class ReturnToMainMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        GameEvents.LevelExit();
    }
}
